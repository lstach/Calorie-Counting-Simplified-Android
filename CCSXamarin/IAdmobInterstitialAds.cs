using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CCSXamarin
{
    public interface IAdmobInterstitialAds
    {
        Task Display(string adID);
    }
}
