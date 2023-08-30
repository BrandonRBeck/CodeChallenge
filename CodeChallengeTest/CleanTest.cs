using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Xunit.Abstractions;




namespace CodeChallengeTest
{
    public class CleanTest
    {
        private readonly ITestOutputHelper _output;

        public CleanTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        private void RunTest()
        {
            //var friendsList = GetMutualFriends(1,2);

            //Assert.Equal(new List<int> { 4, 7, 9, 14, 23, 27, 34 }, friendsList.OrderBy(x => x).ToList());

            var suggested = GetSuggestedFriends(1, 10);

            foreach(var friend in suggested)
            {
                _output.WriteLine($"Friend Id: {friend.Key}, Friend common count {friend.Value}");

            }
        }

        private static List<int> GetFriends(int user_id)
        {
            if (TEST_CASE.TryGetValue(user_id, out var friends))
            {
                return friends;
            }
            throw new Exception("User not found.");
        }

        // Returns a list of user ids that are friends with both users.
        private static List<int> GetMutualFriends(int user_one, int user_two)
        {
            var mutualFriendList = new List<int>();

            var friends2 = GetFriends(user_two);

            foreach (var friend in GetFriends(user_one))
            {
                if (friends2.Contains(friend)){
                    mutualFriendList.Add(friend);
                }
            }
            return mutualFriendList;
        }

        // Returns a list of the top k suggested friends for the user.
        private static Dictionary<int, int> GetSuggestedFriends(int user_id, int k)
        {
            var currentFriends = GetFriends(user_id);
            var commonFriendOccurance = new Dictionary<int, int>();
            
            foreach (var friend in currentFriends)
            {
                var friendsofFriend = GetFriends(friend);
                foreach(var fof in friendsofFriend)
                {
                    if(fof == user_id || currentFriends.Contains(fof))
                    {
                        continue;
                    }
                    if (commonFriendOccurance.ContainsKey(fof)){
                        commonFriendOccurance[fof]++;
                    }
                    else
                    {
                        commonFriendOccurance[fof] = 1;
                    }
                }
            }
            return commonFriendOccurance.OrderByDescending(x => x.Value).Take(k).ToDictionary(x=>x.Key, x=>x.Value);
        }


        private static readonly Dictionary<int, List<int>> TEST_CASE = new Dictionary<int, List<int>>
        {
            {1, new List<int>{23, 36, 27, 8, 34, 9, 4, 3, 39, 2, 7, 11, 12, 13, 14, 24, 28, 38}},
            {2, new List<int>{45, 17, 34, 1, 46, 15, 9, 19, 27, 4, 7, 14, 18, 23, 25, 32, 50}},
            {3, new List<int>{1, 38, 35, 39, 40, 50, 4, 44, 11, 12, 13, 14, 18, 21, 27, 28, 29, 42, 45, 46}},
            {4, new List<int>{1, 2, 3, 34, 36, 21, 25, 8, 46, 15, 42, 17, 19, 29, 37, 39}},
            {5, new List<int>{29, 42, 26, 46, 49, 23, 34, 7, 47, 38, 13, 15, 18, 20, 32, 39, 50}},
            {6, new List<int>{11, 24, 45, 14, 22, 8, 47, 37, 48, 9, 10, 28, 35, 38, 46, 49, 50}},
            {7, new List<int>{5, 11, 8, 2, 24, 28, 41, 50, 19, 1, 9, 12, 18, 21, 22, 23, 26, 42}},
            {8, new List<int>{1, 4, 6, 7, 16, 24, 37, 17, 13, 33, 28, 25, 50, 19, 26, 31, 34, 35, 38, 40, 41, 44}},
            {9, new List<int>{1, 2, 27, 6, 23, 43, 7, 25, 14, 10, 49, 37, 11, 24, 44}},
            {10, new List<int>{9, 16, 21, 18, 32, 6, 24, 29, 13, 14, 33, 42}},
            {11, new List<int>{6, 7, 16, 42, 37, 3, 9, 41, 1, 19, 24, 27, 38, 39, 49}},
            {12, new List<int>{35, 41, 34, 7, 3, 1, 24, 37, 22, 19, 45, 46}},
            {13, new List<int>{8, 10, 41, 24, 21, 43, 39, 3, 14, 1, 5, 16, 42, 44, 46}},
            {14, new List<int>{6, 9, 13, 1, 10, 35, 3, 2, 41, 36, 21, 16, 17, 26, 27, 42}},
            {15, new List<int>{2, 4, 5, 42, 39, 48, 24, 45, 18, 17, 29, 35, 37, 38, 43, 44, 50}},
            {16, new List<int>{8, 10, 11, 14, 28, 19, 17, 47, 35, 49, 39, 13, 21, 23, 26, 29, 31}},
            {17, new List<int>{2, 8, 16, 19, 14, 26, 38, 48, 4, 15, 25, 31, 18, 22, 30, 33, 34, 44, 50}},
            {18, new List<int>{10, 15, 17, 2, 7, 5, 30, 50, 48, 49, 3, 21, 19, 22, 29, 32, 47}},
            {19, new List<int>{2, 7, 11, 16, 17, 12, 4, 45, 8, 28, 38, 50, 18, 39, 25, 34, 37, 46, 49}},
            {20, new List<int>{49, 30, 50, 26, 47, 32, 40, 42, 5, 24, 33, 39}},
            {21, new List<int>{4, 10, 13, 14, 18, 47, 3, 22, 35, 43, 7, 36, 42, 16, 26, 34, 39}},
            {22, new List<int>{6, 12, 21, 7, 25, 32, 17, 18, 41, 39, 50, 33, 40, 46, 49}},
            {23, new List<int>{1, 5, 9, 37, 2, 49, 50, 16, 7, 29, 31, 27, 28, 39, 41, 42, 43, 47}},
            {24, new List<int>{6, 7, 8, 10, 11, 12, 13, 15, 9, 33, 20, 46, 27, 1, 31, 29, 30, 40, 48}},
            {25, new List<int>{4, 8, 9, 17, 22, 33, 41, 2, 39, 48, 19, 36, 47, 46, 28, 32, 45}},
            {26, new List<int>{5, 17, 20, 42, 21, 7, 14, 8, 16, 33, 27, 36, 43, 49, 50}},
            {27, new List<int>{1, 2, 9, 24, 48, 23, 26, 38, 33, 14, 36, 3, 11, 28, 47}},
            {28, new List<int>{7, 8, 16, 19, 6, 36, 25, 27, 3, 39, 29, 23, 1, 30}},
            {29, new List<int>{5, 10, 23, 24, 28, 16, 15, 34, 4, 40, 3, 32, 33, 46, 18, 30, 35, 36, 37, 38, 41}},
            {30, new List<int>{18, 20, 24, 41, 29, 47, 17, 38, 33, 28, 32, 39}},
            {31, new List<int>{17, 23, 24, 36, 8, 35, 33, 46, 50, 37, 16, 47, 34, 41, 43, 49}},
            {32, new List<int>{10, 20, 22, 29, 30, 37, 18, 25, 5, 2, 39, 46, 35, 44, 48}},
            {33, new List<int>{8, 24, 25, 26, 27, 29, 30, 31, 37, 22, 17, 38, 10, 47, 20}},
            {34, new List<int>{1, 2, 4, 5, 12, 29, 31, 46, 17, 8, 42, 19, 21, 45, 36, 44}},
            {35, new List<int>{3, 12, 14, 16, 21, 31, 48, 46, 29, 15, 32, 36, 8, 6, 37, 41}},
            {36, new List<int>{1, 4, 14, 21, 25, 27, 28, 31, 35, 34, 41, 43, 38, 29, 47, 26, 40, 45, 48}},
            {37, new List<int>{6, 8, 9, 11, 12, 23, 31, 32, 33, 4, 46, 15, 50, 44, 35, 19, 29, 40, 42}},
            {38, new List<int>{3, 5, 17, 19, 27, 30, 33, 36, 39, 15, 11, 29, 1, 41, 6, 8, 49}},
            {39, new List<int>{1, 3, 13, 15, 16, 19, 22, 25, 28, 32, 38, 4, 11, 21, 5, 20, 43, 30, 23, 40, 44, 47, 49}},
            {40, new List<int>{3, 20, 29, 37, 8, 36, 42, 49, 24, 39, 22, 41}},
            {41, new List<int>{7, 11, 12, 13, 14, 22, 25, 30, 36, 38, 40, 23, 42, 8, 47, 46, 31, 35, 29}},
            {42, new List<int>{4, 5, 11, 15, 20, 21, 26, 34, 40, 41, 23, 37, 10, 13, 7, 14, 3, 44, 43}},
            {43, new List<int>{9, 13, 21, 36, 39, 31, 15, 42, 23, 26, 45, 48}},
            {44, new List<int>{3, 37, 42, 15, 32, 48, 39, 9, 34, 17, 8, 13, 45}},
            {45, new List<int>{2, 6, 15, 19, 34, 44, 3, 25, 12, 47, 36, 43, 51}},
            {46, new List<int>{2, 4, 5, 24, 25, 29, 31, 32, 34, 35, 37, 41, 12, 22, 3, 13, 6, 19}},
            {47, new List<int>{5, 6, 16, 20, 21, 25, 30, 31, 33, 36, 41, 45, 27, 23, 18, 39, 49}},
            {48, new List<int>{6, 15, 17, 18, 25, 27, 35, 44, 49, 24, 36, 32, 50, 43}},
            {49, new List<int>{5, 9, 16, 18, 20, 23, 38, 40, 48, 22, 19, 47, 6, 11, 39, 31, 26}},
            {50, new List<int>{3, 7, 8, 18, 19, 20, 22, 23, 31, 37, 48, 2, 5, 6, 26, 15, 17}},
            {51, new List<int>{52, 45}},
            {52, new List<int>{51, 53}},
            {53, new List<int>{52}}
        };
    }
}
