﻿// Copyright (c) Microsoft. All rights reserved.

using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.SkillDefinition;

namespace GBB.Miyagi.RecommendationService.plugins;

/// <summary>
///     UserProfilePlugin shows a native skill example to look user info given userId.
/// </summary>
/// <example>
///     Usage: kernel.ImportSkill("UserProfilePlugin", new UserProfilePlugin());
///     Examples:
///     SKContext["userId"] = "000"
///     {{UserProfilePlugin.GetUserAge $userId }} => {userProfile}
/// </example>
public class UserProfilePlugin
{
    /// <summary>
    ///     Name of the context variable used for UserId.
    /// </summary>
    public const string UserId = "UserId";

    private const string DefaultUserId = "40";
    private const int DefaultAnnualHouseholdIncome = 150000;
    private const int Normalize = 81;

    /// <summary>
    ///     Lookup User's age for a given UserId.
    /// </summary>
    /// <example>
    ///     SKContext[UserProfilePlugin.UserId] = "000"
    /// </example>
    /// <param name="context">Contains the context variables.</param>
    [SKFunction("Given a userId, get user age")]
    [SKFunctionName("GetUserAge")]
    [SKFunctionContextParameter(Name = UserId, Description = "UserId", DefaultValue = DefaultUserId)]
    public string GetUserAge(SKContext context)
    {
        var userId = context.Variables.ContainsKey(UserId) ? context[UserId] : DefaultUserId;
        context.Log.LogDebug("Returning hard coded age for {0}", userId);

        int parsedUserId;
        int age;

        if (int.TryParse(userId, out parsedUserId))
        {
            age = parsedUserId > 100 ? (parsedUserId % Normalize) : parsedUserId;
        }
        else
        {
            age = int.Parse(DefaultUserId);
        }

        // invoke a service to get the age of the user, given the userId
        return age.ToString();
    }
    
    /// <summary>
    ///     Lookup User's annual income given UserId.
    /// </summary>
    /// <example>
    ///     SKContext[UserProfilePlugin.UserId] = "000"
    /// </example>
    /// <param name="context">Contains the context variables.</param>
    [SKFunction("Given a userId, get user annual household income")]
    [SKFunctionName("GetAnnualHouseholdIncome")]
    [SKFunctionContextParameter(Name = UserId, Description = "UserId", DefaultValue = DefaultUserId)]
    public string GetAnnualHouseholdIncome(SKContext context)
    {
        var userId = context.Variables.ContainsKey(UserId) ? context[UserId] : DefaultUserId;
        context.Log.LogDebug("Returning userId * randomMultiplier for {0}", userId);

        var random = new Random();
        var randomMultiplier = random.Next(1000, 8000);

        // invoke a service to get the annual household income of the user, given the userId
        var annualHouseholdIncome = int.TryParse(userId, out var parsedUserId)
            ? parsedUserId * randomMultiplier
            : DefaultAnnualHouseholdIncome;

        return annualHouseholdIncome.ToString();
    }
}