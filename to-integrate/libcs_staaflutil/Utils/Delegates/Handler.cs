using System;

namespace Fairweather.Service
{
    public delegate void Handler<TMut>(object sender, Args<TMut> eargs);
    public delegate void Handler<TMut, TImm>(object sender, Args<TMut, TImm> eargs);
    public delegate void Handler_RO<TImm>(object sender, Args_RO<TImm> eargs);
}
