
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
    public class InfectionAdapter : RecyclerView.Adapter
    {
        public List<Infection> Infections { get; set; } 

        private Context context;

        public InfectionAdapter(Context context)
            : base()
        {
            this.Infections = new List<Infection>();
            this.context = context;
        }

        public override int ItemCount
        {
            get
            {
                return this.Infections.Count;
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.infection_card, parent, false);
            return new ViewHolder(view);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            Player player = Players[position];
            ViewHolder vh = holder as ViewHolder;
            if (player.Team == GameUtils.Team.Human)
            {
                // vh.PlayerCard.SetCardBackgroundColor(Resource.Color.human); // doesn't work for some reason
                vh.PlayerCard.SetBackgroundResource(Resource.Color.human);
            }
            else
            {
                // vh.PlayerCard.SetCardBackgroundColor(Resource.Color.zombie); // doesn't work for some reason
                vh.PlayerCard.SetBackgroundResource(Resource.Color.zombie);
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
            public TextView InfectionText { get; set; }

            public ViewHolder(View view)
                : base(view)
            {
                this.InfectionText = view.FindViewById<TextView>(Resource.Id.infection_text);
            }
        }
    }
}

