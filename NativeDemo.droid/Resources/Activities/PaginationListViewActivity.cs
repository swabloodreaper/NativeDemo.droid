using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using NativeDemo.droid.Resources.Adaptor;
using NativeDemo.Models;

namespace NativeDemo.droid.Resources.Activities
{
    [Activity(Label = "PaginationListViewActivity")]
    public class PaginationListViewActivity : Activity
    {
        public bool isLoading = false;
        private ListView ListView;
        public CustomListAdapter adapter;
        public ProgressBar progress;
        private List<User> UserList;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Pagination);
            progress = FindViewById<ProgressBar>(Resource.Id.page_progress);
            ListView = FindViewById<ListView>(Resource.Id.page_listview);
            adapter = new CustomListAdapter();

            SwipeRefreshLayout Refresh = FindViewById<SwipeRefreshLayout>(Resource.Id.page_swipeRefresh);
            Refresh.SetProgressBackgroundColorSchemeColor(Android.Resource.Color.HoloOrangeLight);
            Refresh.Refresh += delegate
            {
                adapter.Clear();
                currentIndex = 0;
                BindNextItems();
                Refresh.Refreshing = false;
            };

            UserList = new List<User>();
            for (int i = 0; i < 50; i++)
            {
                UserList.AddRange(new UserData().Users);
            }
            ListView.Adapter = adapter;
            BindNextItems();
            ListView.Scroll += ListView_Scroll;
        }
        private void DisplayProgressBar(bool enable)
        {
            progress.Visibility = enable ? ViewStates.Visible : ViewStates.Gone;
        }

        bool _isBusy = false;
        private async void ListView_Scroll(object sender, AbsListView.ScrollEventArgs e)
        {
            if (_isBusy)
                return;
            var list = sender as ListView;
            var lastIndexInScreen = e.VisibleItemCount + e.FirstVisibleItem;
            if (lastIndexInScreen >= e.TotalItemCount && lastIndexInScreen <50)
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
            var items = UserList.Skip(currentIndex).Take(limit).ToList();

            if (items.Any())
            {
                currentIndex += limit;
          
                adapter.AddList(items);
      
            }
        }
    }
}