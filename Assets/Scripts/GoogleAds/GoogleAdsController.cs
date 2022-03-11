using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System.Linq;
using Cysharp.Threading.Tasks;

public class GoogleAdsController :MonoBehaviour {
    private BannerView bannerView;

    void Start() {
        MobileAds.Initialize(initStatus => {
            this.RequestBanner();
            this.RequestInterstitial();
        });
        previousAdsTime = -adsTime;
    }

    private void RequestBanner() {
#if UNITY_ANDROID
        //New
        string adUnitId = "ca-app-pub-3934473081985848/6322656186";
        //Test
        //string adUnitId = "ca-app-pub-3940256099942544/6300978111";

        // Clean up banner ad before creating a new one.
        if (this.bannerView != null) {
            this.bannerView.Destroy();
        }
        AdSize adaptiveSize =
            AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        this.bannerView = new BannerView(adUnitId, adaptiveSize, AdPosition.Top);

        AdRequest request = new AdRequest.Builder().Build();

        this.bannerView.LoadAd(request);
#endif
    }

    private InterstitialAd interstitial;

    private void RequestInterstitial() {
        Debug.LogWarning("RequestInterstitial");
#if UNITY_ANDROID
        if (interstitial is object) {
            this.interstitial.Destroy();
            this.interstitial = null;
        }

        //Test
        //string adUnitId = "ca-app-pub-3940256099942544/1033173712";
        //
        string adUnitId = "ca-app-pub-3934473081985848/6694971525";

        this.interstitial = new InterstitialAd(adUnitId);
        AdRequest request = new AdRequest.Builder().Build();
        this.interstitial.LoadAd(request);

#endif
    }

    public static GoogleAdsController Get() =>
        GameObject.FindGameObjectsWithTag("Singletons").Single().GetComponent<GoogleAdsController>();

    [ContextMenu("GameOver")]
    public async UniTask GameOver() {
        Debug.LogWarning("GameOver");
        if (Time.time - previousAdsTime >= adsTime) {
            if (this.interstitial is object && this.interstitial.IsLoaded()) {
                var flag = false;
                this.interstitial.OnAdClosed += (_, _) => {
                    RequestInterstitial();
                    flag = true;
                };
                this.interstitial.Show();
                await UniTask.WaitUntil(() => flag == true);
                previousAdsTime = Time.time;
            }
        }
    }
    float previousAdsTime;
    [SerializeField] float adsTime;
}
