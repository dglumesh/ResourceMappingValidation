using GroupManager.Models;
using GroupManager.Utils;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Management.Automation;
using System.IO;
using System.Web.Mvc.Ajax;
using System.Text;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Threading;
using Newtonsoft.Json.Linq;
using GroupManager.TableHandling;
using System.Linq;

namespace GroupManager.Controllers
{
	public class GroupsController : Controller
	{
		// For simplicity, this sample uses an in-memory data store instead of a db.
		private ConcurrentDictionary<string, List<Subscription>> subscriptionList = new ConcurrentDictionary<string, List<Subscription>>();
		private List<Subscription> subscriptionlist = new List<Subscription>();

		[Authorize]
		[HttpGet]
		public ActionResult Index()
		{
			return View();
		}

	
		static async Task<string> SendURI(Uri u, HttpContent c, string token)
		{
			var response = string.Empty;
			using (var client = new HttpClient())
			{

				client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
				HttpRequestMessage request = new HttpRequestMessage
				{
					Method = HttpMethod.Put,
					RequestUri = u,
					Content = c
				};

				HttpResponseMessage result = await client.SendAsync(request);
				if (result.IsSuccessStatusCode)
				{
					result.EnsureSuccessStatusCode();

					string json = await result.Content.ReadAsStringAsync();

					dynamic data = JObject.Parse(json);
					//response = result.StatusCode.ToString();
					response = json;
				}
			}
			return response;
		}

		static async Task<string> SendURIInvite(Uri u, HttpContent c, string token)
		{
			var response = string.Empty;
			using (var client = new HttpClient())
			{

				client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
				HttpRequestMessage request = new HttpRequestMessage
				{
					Method = HttpMethod.Post,
					RequestUri = u,
					Content = c
				};

				HttpResponseMessage result = await client.SendAsync(request);
				if (result.IsSuccessStatusCode)
				{
					result.EnsureSuccessStatusCode();

					string json = await result.Content.ReadAsStringAsync();

					dynamic data = JObject.Parse(json);
					//InviteUserResponse resultresponse = JsonConvert.DeserializeObject<InviteUserResponse>(json);

					//response = result.StatusCode.ToString();
					response = json;
				}
			}
			
			return response;
		}

		static async Task<string> SendURIMoveValidate(Uri u, HttpContent c, string token)
		{
			var response = string.Empty;
			using (var client = new HttpClient())
			{

				client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
				HttpRequestMessage request = new HttpRequestMessage
				{
					Method = HttpMethod.Post,
					RequestUri = u,
					Content = c
				};

				HttpResponseMessage result = await client.SendAsync(request);
				if (result.IsSuccessStatusCode)
				{
					result.EnsureSuccessStatusCode();

					//string json = await result.Content.ReadAsStringAsync();

					//dynamic data = JObject.Parse(json);
					//InviteUserResponse resultresponse = JsonConvert.DeserializeObject<InviteUserResponse>(json);

					//response = result.StatusCode.ToString();
					response = result.ReasonPhrase;
				}
			}

			return response;
		}

		[Authorize]
		public async Task<ActionResult> ResourceMapping()
		{
			string token2 = await GetGraphAccessToken(new string[] { "https://management.core.windows.net//user_impersonation" });



			// Construct the query
			HttpClient client = new HttpClient();
			HttpRequestMessage request2 = new HttpRequestMessage(HttpMethod.Get, Globals.MicrosoftGraphSubscriptionApi);
			request2.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token2);

			// Ensure a successful response
			HttpResponseMessage response2 = await client.SendAsync(request2);
			response2.EnsureSuccessStatusCode();

			// Populate the data store with the first page of groups
			string json2 = await response2.Content.ReadAsStringAsync();
			SubscriptionResponse result2 = JsonConvert.DeserializeObject<SubscriptionResponse>(json2);

			
			dynamic data = JObject.Parse(json2);
			

			ViewBag.sublist = new SelectList(data.value, "subscriptionId", "displayName");

			return View();
		}
		
	


		

		[Authorize]
		[HttpPost]
		public async Task<ActionResult> Generate([Bind(Include = "subscriptionId")] Subscription subscriptiondata)
		{
			Page_Load();

			string tenantId = ClaimsPrincipal.Current.FindFirst(Globals.TenantIdClaimType).Value;
			List<ResourceMoveValidate> ResourceList = new List<ResourceMoveValidate>();
			try
			{
				// Get a token for the Microsoft Graph
				string token2 = await GetGraphAccessToken(new string[] { "https://management.core.windows.net//user_impersonation" });

				// Construct the query
				string urlresourcelist = String.Format(Globals.MicrosoftResourceGraphAPI);
				Uri u1 = new Uri(urlresourcelist);
				var message1 = "{\"subscriptions\": [\""+ subscriptiondata.subscriptionId +"\"]}";
				HttpContent c1 = new StringContent(message1, Encoding.UTF8, "application/json");
				var t1 = Task.Run(() => SendURIInvite(u1, c1, token2));
				t1.Wait();
				var resourcedata = t1.Result;
				dynamic data1 = JObject.Parse(resourcedata);

				

				foreach (var resource in data1.data.rows)
				{
					string urlresourcemovevalidate = String.Format(Globals.MicrosoftResourceMoveValidate, subscriptiondata.subscriptionId, resource[6]);
					Uri u2 = new Uri(urlresourcemovevalidate);
					var message2 = "{\"resources\": [\"" + resource[0] + "\"],\"targetResourceGroup\":\"/subscriptions/a4ee0626-a970-437f-881f-cf401034feb5/resourceGroups/SSLTest\"}";
					HttpContent c2 = new StringContent(message2, Encoding.UTF8, "application/json");
					var t2 = Task.Run(() => SendURIMoveValidate(u2, c2, token2));
					t2.Wait();

					
					ResourceList.Add(new ResourceMoveValidate { ResourceName = resource[1],ResourceType =resource[2], ResourceGroup = resource[6], Status = t2.Result });

				}

			}
			catch (MsalUiRequiredException ex)
			{
				/*
                    If the tokens have expired or become invalid for any reason, ask the user to sign in again.
                    Another cause of this exception is when you restart the app using InMemory cache.
                    It will get wiped out while the user will be authenticated still because of their cookies, requiring the TokenCache to be initialized again
                    through the sign in flow.
                */
				return new RedirectResult("/Account/SignIn");
			}
			// Handle unexpected errors.
			catch (Exception ex)
			{
				return new RedirectResult("/Error?message=" + ex.Message);
			}

			return View(ResourceList);
			

		}

		protected void Page_Load()
		{
			Response.Write("<!DOCTYPE html><html><body><div class=\"none\" style=\"text-align:center;vertical-align:middle;font-family:Verdana;color:Black;position:absolute;top:50%;left:50%;margin-left:-88px;font-size:small;\" id=\"dvProgress1\"runat=\"server\">Please Wait...<img src=\"/Content/Images/Eclipse2.svg\" style =\"vertical-align:middle\" alt =\"Processing\" /></div></body></html>");
			Response.Flush();
		}
		/// <summary>
		/// We obtain access token for Microsoft Graph with the scope "group.read.all". Since this access token was not obtained during the initial sign in process 
		/// (OnAuthorizationCodeReceived), the user will be prompted to consent again.
		/// </summary>
		/// <returns></returns>
		private async Task<string> GetGraphAccessToken(string[] scopes)
		{
			IConfidentialClientApplication cc = MsalAppBuilder.BuildConfidentialClientApplication();
			IAccount userAccount = await cc.GetAccountAsync(ClaimsPrincipal.Current.GetMsalAccountId());

			Microsoft.Identity.Client.AuthenticationResult result = await cc.AcquireTokenSilent(scopes, userAccount).ExecuteAsync();
			return result.AccessToken;
		}


	}
}
