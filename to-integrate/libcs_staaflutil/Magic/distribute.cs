

using System;
using System.Collections.Generic;
using System.Diagnostics;

using System.Linq;


namespace Fairweather.Service
{
    static partial class Magic
    {



        [Obsolete("Use distribute instead")]
        /// <summary>
        /// This routine allocates a sequence of credit amounts towards
        /// a sequence of debit amounts.
        /// "next_credit" should return the next credit amount, or null if
        ///     there are no more credit amounts to allocate.
        /// "next_debit" should return the next debit amount, or null
        ///     if there are no more debit amounts to cover.
        /// "on_allocated" is called every time a part of a credit amount is allocated
        ///     toward a part of a debit amount, with the following parameters:
        ///     "amount" - the allocation amount ( = min(current_credit, current_debit))
        ///     "drained" - whether the current credit amount has been exhausted (and a call to next_credit
        ///         is imminent)
        ///     "covered" - whether the current debit amount has been satisfied (and a call to next_debit is
        ///         imminent)
        ///         
        /// the return value indicates whether the credit amounts have covered the debit amounts exactly.
        /// </summary>
        public static bool
        Allocate_Credits_To_Debits(Func<decimal?> next_credit,
                                   Func<decimal?> next_debit,
                                   Allocator allocate) {


            decimal credit, debit;
            decimal? null_credit = null, null_debit = null;

            //while((null_outstanding = (null_outstanding ?? next_outstanding())).HasValue){

            bool covered = true;
            bool drained = true;

            // get outstanding
            while (null_debit.HasValue || (null_debit = next_debit()).HasValue) {

                if (null_debit == 0.0m) {
                    null_debit = null;
                    continue;
                }

                covered = false;

                // get credit
                if (null_credit.HasValue || (null_credit = next_credit()).HasValue) {

                    if (null_credit == 0.0m) {
                        null_credit = null;
                        continue;
                    }

                    debit = null_debit.Value;
                    credit = null_credit.Value;

                    covered = debit <= credit;
                    drained = debit >= credit;

                    // amount to allocate
                    var amount = Math.Min(debit, credit);

                    allocate(amount, drained, covered);

                    null_debit -= amount;
                    null_credit -= amount;

                    if (covered)
                        null_debit = null;

                    if (drained)
                        null_credit = null;

                    continue;
                }

                break;

            }

            return covered && drained;

        }

        public static void
        distribute(Func<decimal?> next_credit,
                   Func<decimal?> next_debit,
                   Action<decimal> allocate) {

            distribute(next_credit, next_debit, (_d, _1, _2) => allocate(_d));

        }

        /// <summary>
        /// Distributes a sequence of amounts (represented by next_giver)
        /// to a sequence of deficits (represented by next_taker), making sure
        /// that leftover amounts after an allocation are properly allocated to
        /// the next deficit and that no deficits are skipped.
        /// trivia: exchanging 'next_giver' & 'next_taker' produces the same behavior apart
        /// from the bool parameters to 'allocate' (which are of course themselves exchanged)
        /// </summary>
        public static void
        distribute(Func<decimal?> next_giver,
                   Func<decimal?> next_taker,
                   Allocator allocate) {

            decimal? to_take;
            decimal? to_give = 0.0m;

            while ((to_take = next_taker()) != null) {

                if (to_take == 0.0m)
                    continue; // next taker


                while (true) {

                    while (to_give == 0.0m) {
                        to_give = next_giver();
                        if (to_give == null)
                            return; // nothing more to give

                    }

                    if (to_give < to_take) {
                        allocate(to_give.Value, true, false);
                        to_take -= to_give.Value;
                        to_give = 0.0m;
                        continue; // next giver

                    }
                    else if (to_give > to_take) {
                        allocate(to_take.Value, false, true);
                        to_give -= to_take.Value;
                        to_take = 0.0m;
                        break; // next taker

                    }
                    else /*(to_give == to_take)*/ {
                        allocate(to_take.Value, true, true);
                        to_take = 0.0m;
                        to_give = 0.0m;
                        break; // next taker & giver

                    }

                }

            }

            // nothing more to take

        }
    }
}