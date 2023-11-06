using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Poker;
using Poker.Controller;
using Poker.Interfaces;
using Poker.Models;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
// todo: update SystemGamePlayers with DatabaseGamePlayers loading data from db
builder.Services.AddSingleton<IPlayer, SystemGamePlayers>();
builder.Services.AddSingleton<IPokerGame, PokerGame>();

await builder.Build().RunAsync();