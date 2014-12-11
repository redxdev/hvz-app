
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
            var infection = Infections[position];
            ViewHolder vh = holder as ViewHolder;

            vh.InfectionText.Text = string.Format(
                "{0} was infected by {1} {2}",
                infection.HumanName,
                infection.ZombieName,
                TimeUtils.PrettyDate(infection.Time)
                );
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

