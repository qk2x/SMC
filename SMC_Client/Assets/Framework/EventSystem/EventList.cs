public enum EventName
{
    /// <summary>
    /// 游戏重启
    /// </summary>
    GameRestart = -2,
    GameReBoot = -1,

    InvalidEvent = 0,

    AnyButtonClick,
    ScrollRectPageChange,
    UIOpen,
    UIOpenOver,
    UIClose,
    OnApplicationPause,
    OnDebugInfoHide,
    
    GetPageDataError,
    
    #region Net

    NetworkConnected,
    NetworkDisconnect,
    NetworkConnectFailed,
    NetworkReconnected,
    NetworkTimeOut,

    #endregion

    #region Login

    LoginFailed,

    #endregion

    #region Chat

    ChatInitOk,
    ChatCreatePreviewMessage,
    ChatReceiveHistoryMsg,
    ChatReceiveNewMsg,

    ChatSessionChange,
    ChatSelectEmoji,
    ChatSelectEmojiPack,
    ChatItemLeftMove,
    ChatItemHideMenu,

    ChatRecordTranslate,
    ChatTranslateSuccess,

    ChatUpdateSession,
    ChatDeleteSession,

    ChatAddRedPoint,
    ChatDelRedPoint,

    ConnectedChatRoom,
    OnSessionUpdate,
    
    StartChatSession,
    ChatEmojiAdd,
    ChangeChatPage,
    ShowChatBubble,
    #endregion

    #region Player Info

    //界面
    HeadSelected,
    SelectHeadUpdate,
    FlagSelected,

    // 消息通知刷新
    SelfInfoUpdate,
    SelfAvatarUpdate,
    SelfSocialInfoUpdate,
    SelfNameError,
    
    SelfPlatformConnectionError,
    SelfPlatformConnectionUpdate,
    SelfPlatformConnectionListUpdate,
    
    PlayerFullInfoUpdate,

    AccountInfoError,
    AccountInfoUpdate,
    RevokeDeviceSuccess,
    
    PlayerCustomAvatarError,
    PlayerProfilePanelSwitch,
    
    PlayerNameChange,
    #endregion
    
    #region Character Appearance

    AppearanceItemSelected,
    EmoteSlotSelected,
    AppearanceComponentsUpdate,
    AppearanceBundlesUpdate,
    AppearanceSlotUpdate,

    PoseSphereClick,
    
    #endregion

    #region Relationship

    PlayerRelationshipStateUpdate,
    PlayerFollowListUpdate,
    SearchResultUpdate,
    
    PlayerLikeInfoUpdate,
    PlayerLikeHistoryUpdate,
    
    BlockedPlayerBriefListUpdate,
    BlockPlayerFailed,

    #endregion

    #region Player Referrals

    ReferralsBasicInfoUpdate,
    ReferralsEarningInfoUpdate,
    ReferralsUserNumInfoUpdate,

    #endregion

    #region Robux Exchange
    
    ExchangeMaintenanceStateUpdate,
    RecentExchangeHistoryListUpdate,
    SelfExchangeHistoryListUpdate,
    ExchangeRobloxAccountListUpdate,
    ExchangeRobloxAccountSelected,
    JumpToExchangeHistoryTab,
    WithdrawRobuxStateUpdate,
    RobuxCheckItemListUpdate,
    ExchangeDefaultPayMethodUpdate,
    ExchangePayMethodStateUpdate,

    #endregion
    
    #region Res & Item

    PlayerResChange,
    PlayerRoblingChange,
    PlayerDiamondChange,
    
    #endregion

    #region Store

    RefreshStoreItem,
    BuyStoreItemSuccess,
    CustomizeProductSelectUpdate,
    CustomizeProductRemoved,
    CustomizeProductPreviewNumUpdate,

    #endregion
    
    #region Tip

    GridItemResGainTipFinish, //通用获得 票动到达完成 参数1 PopGainTipInfo
    OnClickShowTipBubble,
    
    OnNewsTickerUpdate,

    #endregion
    
    #region Mail

    MailListUpdate,
    MailSelected,
    MailRead,
    MailClaimed,
    MailDeleted,
    MailStateUpdate,
    MailNeedToNotify,
    MailContentUpdate,
    MailReadClaimedRefresh,

    // For Ranking List
    MailContentLayoutChange,

    #endregion
    
    ShareComplete,
    ShareToAppComplete,

    #region Lobby
    
    PlayLuckyWheel,
    ClaimLuckyWheelReward,
    ResetLuckyWheel,
    
    LuckyWheelSelfDataUpdate,
    LuckyWheelJackpotDataUpdate,
    LuckyWheelHistoryDataUpdate,
    
    RankingExtraDisplayRemove,
    RankingExtraDisplayAdd,
    
    OfferWallRewardsStateUpdate,
    
    #endregion

    #region Party Game

    ParkourRacingShowScene,
    ParkourRacingCountDownStart,
    ParkourRacingStart,
    ParkourRacingRoundEnd,
    PlayerFinishedParkourRacing,
    ParkourRacingFinished,

    #endregion

    #region Ranking

    RankingInfoUpdate,
    SelectedRankingItem,

    #endregion

    #region GameCreator

    SelectGameCreatorItem,

    #endregion

    #region Scene Interaction

    OnDoInteractionDone,
    OnStopInteractionDone,
    
    #endregion

    #region GameCenter

    GameCenterListUpdate,
    MiniGameInfoUpdate,
    MiniGameModeInfoSelect,
    MiniGameModeInfoUpdate,

    #endregion
    
    AdMobCountChange,
    AdChannelChange,
    AdBannerShow,
    
    AdNOfferWallSettingUpdate,
    
    ShopItemStateChange,
    SelectedShopTab,
    
    ReviewGuideStateChange,
    
    ClickNPC,
    MaskChange,
    UIStoryPlay,
    Fire,
    Score,
    EnterGoal,
    ExitGoal,
    MiniGameStart,
    
    #region GameEvent_Action
    //General_GameEvent_ActionPreset,
    EventActionWheelReward, //1001001
    EventActionBeLiked, //2001001
    EventActionChat, //3001001
    EventActionEmojiAnim, //4001001
    EventActionLikeOther, //5001001
    EventActionFollow, //6001001
    EventActionWheelOnce, //7001001
    EventActionApperacnceBuy, //8001001
    EventActionApperacnceChangeAndSave, //8001002
    EventActionOpenOfferwall, //9001001
    EventActionOpenReferral, //9001002,
    EventActionOpenRobuxExchange, //9001003
    
    EventActionChangeName, //200100101 
    EventActionChangeAvatar, // 200100201 修改头像
    EventActionChangeAlias, // 200100301 修改头像
    #endregion

    #region GamePlayTask
    GamePlayTaskProgress, //任务系统进度发生改变
    GameLoginTaskProgress, //任务系统进度发生改变

    #endregion
}