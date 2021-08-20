using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ExceptionHandler;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using ModernHttpClient;
using VideoFilesChecker;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace VideoChecking
{
    public static class CreateHttpRequest
    {
        private static readonly string serverUrl = Program.settings.ServerUrl;

        private static HttpClient client = new HttpClient();
        private static GraphQLHttpClient graphQLClient = new GraphQLHttpClient(new GraphQLHttpClientOptions
        {
            EndPoint = new Uri(serverUrl + "/graphql", UriKind.Absolute),
            HttpMessageHandler = new NativeMessageHandler(),
        }, new NewtonsoftJsonSerializer());

        public static async Task CreatePOSTRequest()
        {
            await PostData();
        }

        public static async Task CreateGETRequest()
        {
            await GetJsonData();
        }

        private static async Task PostData()
        {
            GraphQLRequest createMovies = new GraphQLRequest
            {
                Query = @" mutation createMovies($movies: [MovieInput]) {
                            createMovies(Movies: $movies) {
                              message
                            },
                          }",
                OperationName = "createMovies",
                Variables = new
                {
                    Movies = Program.listOfMovies,
                }
            };

            //GraphQLRequest createTvShow = new GraphQLRequest
            //{
            //    Query = @" mutation createTvShows($tvShows: [TvShowInput]) {
            //                createTvShows(TvShows: $tvShows) {
            //                  message
            //                },
            //              }",
            //    OperationName = "createTvShows",
            //    Variables = new
            //    {
            //        TvShows = Program.listOfTvShows,
            //    }
            //};

            //GraphQLRequest createDocumentaryMovies = new GraphQLRequest
            //{
            //    Query = @" mutation createDocumentaryMovies($documentaryMovies: [MovieInput]) {
            //                createDocumentaryMovies(DocumentaryMovies: $documentaryMovies) {
            //                  message
            //                },
            //              }",
            //    OperationName = "createDocumentaryMovies",
            //    Variables = new
            //    {
            //        DocumentaryMovies = Program.listOfDocumentaryMovies,
            //    }
            //};

            //GraphQLRequest createDocumentaryTv = new GraphQLRequest
            //{
            //    Query = @" mutation createDocumentaryTvShows($documentaryTv: [TvShowInput]) {
            //                createDocumentaryTvShows(DocumentaryTv: $documentaryTv) {
            //                  message
            //                },
            //              }",
            //    OperationName = "createDocumentaryTvShows",
            //    Variables = new
            //    {
            //        DocumentaryTv = Program.listOfDocumentaryTv,
            //    }
            //};

            //GraphQLRequest createDrives = new GraphQLRequest
            //{
            //    Query = @" mutation createDrives($drives: [DriveInput]) {
            //                createDrives(Drives: $drives) {
            //                  message
            //                },
            //              }",
            //    OperationName = "createDrives",
            //    Variables = new
            //    {
            //        Drives = Program.listOfDrives
            //    }
            //};

            var graphQLCreateMoviesResponse = await graphQLClient.SendQueryAsync<CreateMoviesResponseModel>(createMovies);
            //var graphQLCreateTvShowsResponse = await graphQLClient.SendQueryAsync<CreateTvShowsResponseModel>(createTvShow);
            //var graphQLCreateDocumentaryMoviesResponse = await graphQLClient.SendQueryAsync<CreateDocumentaryMoviesResponseModel>(createDocumentaryMovies);
            //var graphQLCreateDocumentaryTvResponse = await graphQLClient.SendQueryAsync<CreateDocumentaryTvResponseModel>(createDocumentaryTv);
            //var graphQLCreateDrivesResponse = await graphQLClient.SendQueryAsync<CreateDrivesResponseModel>(createDrives);

            Console.WriteLine("POST: " + graphQLCreateMoviesResponse.Data.CreateMovies.Message + Environment.NewLine);
            //Console.WriteLine("POST: " + graphQLCreateTvShowsResponse.Data.CreateTvShows.Message + Environment.NewLine);
            //Console.WriteLine("POST: " + graphQLCreateDocumentaryMoviesResponse.Data.CreateDocumentaryMovies.Message + Environment.NewLine);
            //Console.WriteLine("POST: " + graphQLCreateDocumentaryTvResponse.Data.CreateDocumentaryTv.Message + Environment.NewLine);
            //Console.WriteLine("POST: " + graphQLCreateDrivesResponse.Data.CreateDrives.Message + Environment.NewLine);
        }

        private static async Task GetJsonData()
    {
            Console.WriteLine("GET: Starting request.\n");

            var msg = new GraphQLRequest
            {
                Query = @" {
                       getMediaForDeletion{
                            movies {
                              title
                              year
                            }
                            tvShows {
                              title
                              year
                            }
                            documentaryTv {
                              title
                              year
                            }
                            documentaryMovies {
    	                      title
                              year
                            }
                          } 
                    }",
            };

            try
            {
                GraphQLResponse<dynamic> graphQLGetMediaForDeletionRawResponse = await graphQLClient.SendQueryAsync<dynamic>(msg);
                MediaForDeletionModel graphQLGetMediaForDeletionResponse = 
                    JsonConvert.DeserializeObject<MediaForDeletionModel>(JsonConvert.SerializeObject(graphQLGetMediaForDeletionRawResponse.Data));

                VideoDeletion.listOfDeletionRequests = graphQLGetMediaForDeletionResponse.MediaForDeletion;

                Console.WriteLine(JsonConvert.SerializeObject(graphQLGetMediaForDeletionRawResponse.Data, Formatting.Indented) + Environment.NewLine);
            }
            catch (Exception UnableToGetDataException)
            {
                Exceptions.LogException(UnableToGetDataException);
            }

            Console.WriteLine("GET: Request completed." + Environment.NewLine);
        }

        public static async Task Login()
        {
            GraphQLRequest login = new GraphQLRequest
            {
                Query = @" mutation login($emailAddress: String, $password: String) {
                            login(emailAddress: $emailAddress, password: $password) {
                              accessToken
                              refreshToken
                            },
                          }",
                OperationName = "login",
                Variables = new
                {
                    emailAddress = Program.settings.EmailAddress,
                    password = Program.settings.Password
                }
            };

            GraphQLResponse<LoginModel> graphQLLoginResponse = await graphQLClient.SendQueryAsync<LoginModel>(login);
            if (graphQLLoginResponse.Errors == null)
            {
                Console.WriteLine("POST: Login successsful" + Environment.NewLine);
            }
            Program.settings.AccessToken = graphQLLoginResponse.Data.Login.AccessToken;
            Program.settings.RefreshToken = graphQLLoginResponse.Data.Login.RefreshToken;
            graphQLClient.HttpClient.DefaultRequestHeaders.Add("authorization", "Bearer " + graphQLLoginResponse.Data.Login.AccessToken);
        }

        public static async Task Logout()
        {
            GraphQLRequest logout = new GraphQLRequest
            {
                Query = @" mutation logout($refreshToken: String) {
                            logout(refreshToken: $refreshToken) {
                              message
                            },
                          }",
                OperationName = "logout",
                Variables = new
                {
                    Program.settings.RefreshToken
                }
            };

            GraphQLResponse<LogoutResponseModel> graphQLLogoutResponse = await graphQLClient.SendQueryAsync<LogoutResponseModel>(logout);
            Console.WriteLine("POST: " + graphQLLogoutResponse.Data.Logout.Message + Environment.NewLine);
        }
    }
}
