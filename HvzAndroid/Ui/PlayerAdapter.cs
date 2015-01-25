
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;

using Hvz.Api;
using Hvz.Api.Game;
using Hvz.Api.Response;

namespace Hvz.Ui
{
    public class PlayerAdapter : RecyclerView.Adapter
    {
        public List<Player> Players { get; set; }

        private Context context;

        public PlayerAdapter(Context context)
            : base()
        {
            this.Players = new List<Player>();
            this.context = context;
        }

        public override int ItemCount
        {
            get
            {
                return this.Players.Count;
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.player_card, parent, false);
            return new ViewHolder(view);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            Player player = Players[position];
            ViewHolder vh = holder as ViewHolder;
            if (player.Team == GameUtils.Team.Human)
            {
                // vh.PlayerCard.SetCardBackgroundColor(Resource.Color.human); // doesn't work for some reason
                vh.PlayerCardLayout.SetBackgroundResource(Resource.Color.human);
            }
            else
            {
                // vh.PlayerCard.SetCardBackgroundColor(Resource.Color.zombie); // doesn't work for some reason
                vh.PlayerCardLayout.SetBackgroundResource(Resource.Color.zombie);
            }

            vh.PlayerName.Text = player.FullName;
            vh.HumanTags.Text = "Tags: " + player.HumansTagged.ToString();
            vh.Clan.Text = "Clan: " + (string.IsNullOrWhiteSpace(player.Clan) ? "none" : player.Clan);

            if (string.IsNullOrWhiteSpace(player.Avatar))
            {
                vh.AvatarImage.SetImageResource(Resource.Drawable.placeholder);
            }
            else
            {
                Koush.UrlImageViewHelper.SetUrlDrawable(vh.AvatarImage, ApiConfiguration.BaseUrl + "/" + player.Avatar, Resource.Drawable.placeholder);
            }
        }

        private class ViewHolder : RecyclerView.ViewHolder
        {
            public CardView PlayerCard { get; private set; }

            public RelativeLayout PlayerCardLayout { get; private set; }

            public ImageView AvatarImage { get; private set; }

            public TextView PlayerName { get; private set; }

            public TextView HumanTags { get; private set; }

            public TextView Clan { get; private set; }

            public ViewHolder(View view)
                : base(view)
            {
                this.PlayerCard = view.FindViewById<CardView>(Resource.Id.player_card);
                this.PlayerCardLayout = view.FindViewById<RelativeLayout>(Resource.Id.player_card_layout);
                this.AvatarImage = view.FindViewById<ImageView>(Resource.Id.avatar_image);
                this.PlayerName = view.FindViewById<TextView>(Resource.Id.player_name);
                this.HumanTags = view.FindViewById<TextView>(Resource.Id.human_tags);
                this.Clan = view.FindViewById<TextView>(Resource.Id.clan);
            }
        }
    }
}

