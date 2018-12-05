namespace GreetNGroup.Claim_Controls
{
    public static class ClaimsPool
    {
        /// <summary>
        /// This is a pool of claims to be used for authorization
        /// </summary>
        public enum Claims
        {
            SystemAdmin,
            CanViewEvents,
            CanCreateEvents,
            CanFriendUsers,
            CanBlacklistUsers,
            AdminRights,
            Over18
        };
    }
}