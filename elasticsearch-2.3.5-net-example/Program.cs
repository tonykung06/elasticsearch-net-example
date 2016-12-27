using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;

namespace elasticsearch_2._3._5_net_example
{
    class Program
    {
        public static Uri node;
        public static ConnectionSettings settings;
        public static ElasticClient client;

        static void Main(string[] args)
        {
            node = new Uri("http://localhost:9200");
            settings = new ConnectionSettings(node);
            settings.DefaultIndex("my_blog");
            client = new ElasticClient(settings);

            // CreateIndex();
            // InsertData();
            // PerformTermQuery();
            // PerformMatchPhrase();
            PerformFilter();
        }

        public static void CreateIndex()
        {
            var indexState = new IndexState();

            client.CreateIndex("my_blog", c => c
                .Index("my_blog")
                .InitializeUsing(indexState)
                .Mappings(ms => ms.Map<Post>(m => m.AutoMap()))
            );
        }

        public static void InsertData()
        {
            var newBlogPost = new Post
            {
                UserId = 1,
                PostDate = DateTime.Now,
                PostText = "First blog post from NEST!"
            };
            client.Index(newBlogPost);

            var newBlogPost2 = new Post
            {
                UserId = 1,
                PostDate = DateTime.Now,
                PostText = "Second blog post from NEST!"
            };
            client.Index(newBlogPost2);

            var newBlogPost3 = new Post
            {
                UserId = 2,
                PostDate = DateTime.Now,
                PostText = "Third blog post from NEST!"
            };

            var newBlogPost4 = new Post
            {
                UserId = 2,
                PostDate = DateTime.Now.AddDays(5),
                PostText = "Blog post from the future!"
            };

            client.Index(newBlogPost4);
        }

        public static void PerformTermQuery()
        {
            var result = client.Search<Post>(s => s
                .Query(q => q.Term(p => p.PostText, "blog")) 
            );
        }

        public static void PerformMatchPhrase()
        {
            var result = client.Search<Post>(s => s
                .Query(q => q.MatchPhrase(m => m.Field("postText").Query("second blog post")))
            );
        }

        public static void PerformFilter()
        {
            var result = client.Search<Post>(
                s => s.Query(
                    q => q.Term(
                        p => p.PostText,
                        "blog"
                    )
                ).PostFilter(
                    f => f.DateRange(
                        r => r.Field("postDate")
                                .GreaterThan("2016-12-31")
                    )
                )
            );
        }
    }
}
