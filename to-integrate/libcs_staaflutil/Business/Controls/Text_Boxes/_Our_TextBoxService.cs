using System;

using Fairweather.Service;

namespace Common.Controls
{
      internal static class Our_StaticTextBoxBase
      {
            static public void Accept_Char(this ITextBox tb, char ch) {

                  int sel = tb.SelectionStart;
                  int len = tb.SelectionLength;

                  if (len > 0)
                        tb.Text = tb.Text.Remove(sel, len);

                  tb.Text = tb.Text.Insert(sel, ch.ToString());
                  tb.SelectionStart = sel + 1;
            }

            static public bool Set_Text_And_Keep_Selection(this ITextBox tb, string text) {

                  bool ret;

                  int pos = tb.SelectionStart;
                  int sel_len = tb.SelectionLength;

                  tb.Text = text;

                  tb.SelectionStart = Math.Min(pos, tb.Text.Safe_Length());
                  tb.SelectionLength = Math.Min(sel_len, tb.Text.Safe_Length() - tb.SelectionStart);

                  ret = (pos == tb.SelectionStart && sel_len == tb.SelectionLength);

                  return ret;
            }
      }

}