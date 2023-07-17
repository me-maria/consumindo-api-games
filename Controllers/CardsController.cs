using ConsumindoAPIDeGames.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft;
using Newtonsoft.Json;
using System.Globalization;
using X.PagedList;

namespace ConsumindoAPIDeGames.Controllers
{
    public class CardsController : Controller
    {
        private readonly string ENDPOINT = "https://api.magicthegathering.io/v1/cards";
        private readonly HttpClient _httpClient = null;
        private List<Card> _cards = null;

        public CardsController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(ENDPOINT);
        }
        public async  Task<IActionResult> Index()
        {
            MagicCardsViewModel cardList = null;
            _cards = new List<Card>();
            try
            {
                var response = await _httpClient.GetAsync(ENDPOINT);

                if (response.IsSuccessStatusCode) {
                    string content = await response.Content.ReadAsStringAsync();
                    cardList = JsonConvert.DeserializeObject<MagicCardsViewModel>(content);

                    foreach(var card in cardList.cards)
                    {
                        _cards.Add(card);
                    }
                }
                else
                {
                    ModelState.AddModelError(null, "Erro ao processar o Magic!");
                }
            }
            catch (Exception ex)
            {
                String msg = ex.Message;
                throw;
            }
            return View(cardList.cards);
        }
    }
}
