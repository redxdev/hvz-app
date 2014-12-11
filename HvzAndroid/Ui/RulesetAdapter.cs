
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
using Android.Text;
using Android.Webkit;
using Hvz.Api;
using Hvz.Api.Game;
using Hvz.Api.Response;

namespace Hvz.Ui
{
    public class RulesetAdapter : RecyclerView.Adapter
    {
        public List<Ruleset> Rulesets { get; set; } 

        private Context context;

        public RulesetAdapter(Context context)
            : base()
        {
            this.Rulesets = new List<Ruleset>();
            this.context = context;
        }

        public override int ItemCount
        {
            get { return this.Rulesets.Count; }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ruleset_card, parent, false);
            return new ViewHolder(view);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var ruleset = Rulesets[position];
            ViewHolder vh = holder as ViewHolder;
            vh.Title.Text = ruleset.Title;
            vh.Body.Settings.JavaScriptEnabled = false;
            vh.Body.LoadData(ruleset.Body, "text/html", "utf-8");
        }

        private class ViewHolder : RecyclerView.ViewHolder
        {
            public TextView Title { get; set; }

            public WebView Body { get; set; }

            public ViewHolder(View view)
                : base(view)
            {
                this.Title = view.FindViewById<TextView>(Resource.Id.ruleset_title);
                this.Body = view.FindViewById<WebView>(Resource.Id.ruleset_webview);
            }
        }
    }
}

