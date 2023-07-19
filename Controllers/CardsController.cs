using ConsumindoAPIDeGames.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using X.PagedList;

namespace ConsumindoAPIDeGames.Controllers
{
    public class CardsController : Controller
    {
        private readonly string ENDPOINT = "https://api.magicthegathering.io/v1/cards";
        private readonly HttpClient _httpClient = null;
        private static List<Card> _cards = null;

        public CardsController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(ENDPOINT);
        }
        public async  Task<IActionResult> Index(int? page)
        {
            var itemsByPage = 10;
            var currentPage = page ?? 1;


            try
            {
                MagicCardsViewModel cardList = null;
                _cards = new List<Card>();

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
                return View(await cardList.cards.ToPagedListAsync(currentPage, itemsByPage));
            }
            catch (Exception ex)
            {
                String msg = ex.Message;
                throw;
            }

            //return View(cardList.cards);
        }
        [HttpGet]
        public IActionResult Details(string id)
        {
            Card card = _cards.FirstOrDefault(c => c.id.Equals(id));
            return View(card);
        }
    }
}
