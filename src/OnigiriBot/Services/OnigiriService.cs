using Discord;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OnigiriBot.Services
{
    public class OnigiriService
    {
        private readonly HttpClient _http;

        public OnigiriService(HttpClient http)
            => _http = http;

        public async Task<EmbedBuilder> GetOnigiriInfoAsync(string cat = "update", bool details = true)
        {
            string proxyUrl = Environment.GetEnvironmentVariable("OnigiriProxy");
            string logoUrl = Environment.GetEnvironmentVariable("OnigiriLogo");
            var resp = await _http.GetAsync($"{proxyUrl}?t=info_list&cat={cat}&page=1");
            var content = await resp.Content.ReadAsStreamAsync();
            EmbedBuilder embedBuilder = new EmbedBuilder();
            embedBuilder.WithThumbnailUrl(logoUrl);
            using JsonDocument document = await JsonDocument.ParseAsync(content, new JsonDocumentOptions { AllowTrailingCommas = true });
            JsonElement root = document.RootElement;
            JsonElement topics = root.GetProperty("topics");
            foreach (JsonElement topic in topics.EnumerateArray())
            {
                if (topic.TryGetProperty("datetime", out JsonElement datetime))
                {
                    embedBuilder.WithTimestamp(DateTimeOffset.FromUnixTimeSeconds(datetime.GetInt64()));
                }
                if (topic.TryGetProperty("views", out JsonElement views))
                {
                    embedBuilder.AddField("閱讀數", views.GetString());
                }
                if (topic.TryGetProperty("title", out JsonElement title))
                {
                    embedBuilder.Title = title.GetString();
                }
                if (topic.TryGetProperty("url", out JsonElement url))
                {
                    embedBuilder.Url = url.GetString();
                    if (details)
                    {
                        var topicId = embedBuilder.Url.Split(@"/")[^1];
                        var respTopic = await _http.GetAsync($"{proxyUrl}?t=info_topic&cat={cat}&id={topicId}");
                        var contentTopic = await respTopic.Content.ReadAsStreamAsync();
                        using JsonDocument documentTopic = await JsonDocument.ParseAsync(contentTopic, new JsonDocumentOptions { AllowTrailingCommas = true });
                        JsonElement rootTopic = documentTopic.RootElement;
                        HtmlDocument htmlDoc = new HtmlDocument();
                        htmlDoc.LoadHtml(rootTopic.GetProperty("topic").GetProperty("content").GetString());
                        var converter = new ReverseMarkdown.Converter();
                        var markdown = converter.Convert(htmlDoc.DocumentNode.OuterHtml);
                        embedBuilder.Description = htmlDoc.DocumentNode.InnerText;
                    }
                }
                break;
            }
            return embedBuilder;
        }
    }
}
