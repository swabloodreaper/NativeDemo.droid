using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;

namespace NativeDemo.droid.Resources.Activities
{
    [Activity(Label = "ListPaginationActivity")]
    public class PaginationActivity : Activity
    {
        public bool isLoading = false;
        private ListView mListView;
        public ArrayAdapter adapter;
        public ProgressBar progress;
       private List<string> FruitList = Enumerable.Range(0, 1000).Select(x => "Fruit " + (x + 1)).ToList();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Pagination);
            progress = FindViewById<ProgressBar>(Resource.Id.page_progress);
            mListView = FindViewById<ListView>(Resource.Id.page_listview);
            SwipeRefreshLayout Refresh = FindViewById<SwipeRefreshLayout>(Resource.Id.page_swipeRefresh);
            Refresh.SetProgressBackgroundColorSchemeColor(Android.Resource.Color.HoloOrangeLight);
            Refresh.Refresh += delegate
             {
                 adapter.Clear();
                 currentIndex = 0;
                 BindNextItems();
                 Refresh.Refreshing = false;
             };

            
            adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1);
            mListView.Adapter = adapter;
            BindNextItems();
            mListView.Scroll += MListView_Scroll;
        }
        private void DisplayProgressBar(bool enable)
        {
            progress.Visibility = enable ? ViewStates.Visible : ViewStates.Gone;
        }

     
        bool _isBusy = false;
        private async void MListView_Scroll(object sender, AbsListView.ScrollEventArgs e)
        {
            if (_isBusy)
                return;
            var list = sender as ListView;
            var lastIndexInScreen = e.VisibleItemCount + e.FirstVisibleItem;
            if (lastIndexInScreen >= e.TotalItemCount)
            {
                _isBusy = true;
                DisplayProgressBar(true);
             
                await Task.Delay(2000);
                BindNextItems();
                DisplayProgressBar(false);
                _isBusy = false;
            };
        }
        int limit = 15;
        int currentIndex = 0;
        public void BindNextItems()
        {
            var items = FruitList.Skip(currentIndex).Take(limit).ToList();
            if (items.Any())
            {
                currentIndex += limit;
                adapter.AddAll(items);
                adapter.NotifyDataSetChanged();
            }
        }
    }
}