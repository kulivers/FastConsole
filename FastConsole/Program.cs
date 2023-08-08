using System.Xml.Serialization;
using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using CsvHelper;
using FastConsole.models;
using Newtonsoft.Json;

public class Program
{
    public static string[] GetFailedTestsObsolete()
    {
        return new string[]
        {
            "AgentInstanceCountTest",
            "InstanceRestartAfterProcedureEditing",
            "InstanceStartAfterProcedureCreationTest",
            "InstanceStopAfterProcedureDeletionTest",
            "InstanceStopAfterProcedureDisablingTest",
            "AdapterDeletionTest",
            "AdapterPublishingTest",
            "AdapterRepublishingTest",
            "EndpointAndProcedureCreationTest",
            "EndpointDeletionTest",
            "ExternalAdapterMessageSendingTest",
            "InternalAdapterMessageSendingTest",
            "ListDefinitionTest",
            "PasswordExtractingTest",
            "ProcedureDeletionTest",
            "UpdatingEndpointAndProcedureAfterAdapterRepublishingTest",
            "TestAsyncCleanerRace",
            "TestBasics",
            "DatasetConfigurationRemove",
            "DatasetRootObjectCorrectFill",
            "FiltersConjuctionNot",
            "FiltersConjuctionOr",
            "Paging_test",
            "SystemFilterCMWExpressionsTest",
            "UserColumnFIlterSet",
            "UserColumnGroupingSortingAscT",
            "UserColumnSortingAscF",
            "UserColumnSortingDescF",
            "UserColumnSortingDescT",
            "ConvertErrorsTest",
            "DecisionTableTreeStrategyTest",
            "DecisionTreeCrudTest",
            "MemoryUsageTestOnRecordsPerformingTrigger",
            "TestN3SerializerPerformance",
            "BoolFunc_NOTBoolFuncTest",
            "ExpressionsAntlr4GrammarTest",
            "SortingInExpressionsTest",
            "ValueOrDefaultExpressionsTest",
            "WholeBuiltInFunctionsTest",
            "ContextRole_ConditionAlwaysFalse_Deny",
            "ContextRole_ConditionAlwaysTrue",
            "ContextRole_ConditionEnabled_CreateOperation_LocalAdmin_Deny",
            "ContextRole_ConditionEnabled_ModifyOperation_Deny",
            "ContextRole_ConditionEnabled_UpdateOperation",
            "ContextRole_ExpressionEnabled_CreateOperation_LocalAdmin_Deny",
            "ContextRole_ExpressionEnabled_Deny",
            "NestedGroup_ReadModify",
            "NestedGroups_TwoRoles_MoveUser",
            "Solution_CreateOperation",
            "Solution_Deny",
            "Solution_Read_Write_Delete",
            "Task_Read",
            "Template_Deny",
            "Template_LocalAdminFullAccess",
            "Conversation1000Participants",
            "ConversationDraft",
            "ConversationMessageModify",
            "ConversationObjects",
            "Bind_UnBindUserLicenseKeyTest",
            "BulkReadWriteDeleteTest",
            "TestAccountBasics",
            "TestAccountGroupEveryOneGroup",
            "TestAccountIsSystemAdministratorSign",
            "TestCacheClearAfterCreateAccount",
            "TestCommunicationChannelsADFS",
            "TestDateTimeLocal",
            "TestIncludesMemberUser",
            "TestTimezone",
            "TestTimezoneSpan",
            "TestWorkDays",
            "TestWorkDaysTZ",
            "UserTasksAssignee_Test",
            "AccountTemplateTest",
            "GetRoleWorkspaceItemsMTThread",
            "GetRoleWorkspaceWithManyTemplates",
            "CheckObjectHistory",
            "MultiValueRefInExpressionsTest",
            "TestMultiValueProperty",
            "TestObjectPropertyFullTextSearch",
            "WritingIncomingAdapterMessageEvents",
            "TestAttachmentViaLogContents",
            "TestDispose",
            "TestListMatch",
            "TestModelIMultipleInstances",
            "TestPCLImplies",
            "TestPCLIncludes",
            "TestQueryWithContext",
            "TestRealNestedTransaction_Think",
            "TestSequenceWithDecimal",
            "TestYAMLWriter",
            "TrackerTestDynamicModel"
        };
    }

    public static string[] GetFailedTestsNew()
    {
        return new string[]
        {
            "AgentInstanceCountTest",
            "InstanceRestartAfterProcedureEditing",
            "InstanceStartAfterProcedureCreationTest",
            "InstanceStopAfterProcedureDeletionTest",
            "InstanceStopAfterProcedureDisablingTest",
            "AdapterDeletionTest",
            "AdapterPublishingTest",
            "AdapterRepublishingTest",
            "EndpointAndProcedureCreationTest",
            "EndpointDeletionTest",
            "ExternalAdapterMessageSendingTest",
            "ListDefinitionTest",
            "PasswordExtractingTest",
            "ProcedureDeletionTest",
            "UpdatingEndpointAndProcedureAfterAdapterRepublishingTest",
            "TestAsyncCleanerRace",
            "TestBasics",
            "CmwExpressionCompleteTest",
            "FeelCompleteTest",
            "DatasetConfigurationRemove",
            "DatasetRootObjectCorrectFill",
            "FiltersConjuctionNot",
            "FiltersConjuctionOr",
            "Paging_test",
            "SystemFilterCMWExpressionsTest",
            "UserColumnFIlterSet",
            "UserColumnGroupingSortingAscT",
            "UserColumnSortingAscF",
            "UserColumnSortingDescF",
            "UserColumnSortingDescT",
            "ConvertErrorsTest",
            "DecisionTableTreeStrategyTest",
            "DecisionTreeCrudTest",
            "MemoryUsageTestOnRecordsPerformingTrigger",
            "TestN3SerializerPerformance",
            "AccessPropertyViaDollarTest",
            "BackwardAccessTest",
            "BoolFunc_NOTBoolFuncTest",
            "CyrillicAliasesTest",
            "EMPTY_NOTEMPTYTest",
            "ExpressionBug1005825Test",
            "ExpressionBug1118005Test",
            "ExpressionBug476317Test",
            "ExpressionBug982754Test",
            "ExpressionSampleTest",
            "ExpressionsAntlr4ErrorsTest",
            "ExpressionsAntlr4GrammarTest",
            "FeelCompilerTest",
            "MultiValueInExpressionsTest",
            "SortingInExpressionsTest",
            "ValueOrDefaultExpressionsTest",
            "WholeBuiltInFunctionsTest",
            "WorkDaysFunctionsTest",
            "ContextRole_ConditionAlwaysFalse_Deny",
            "ContextRole_ConditionAlwaysTrue",
            "ContextRole_ConditionEnabled_CreateOperation_LocalAdmin_Deny",
            "ContextRole_ConditionEnabled_ModifyOperation_Deny",
            "ContextRole_ConditionEnabled_UpdateOperation",
            "ContextRole_ExpressionEnabled_CreateOperation_LocalAdmin_Deny",
            "ContextRole_ExpressionEnabled_Deny",
            "NestedGroup_ReadModify",
            "NestedGroups_TwoRoles_MoveUser",
            "Solution_CreateOperation",
            "Solution_Deny",
            "Solution_Read_Write_Delete",
            "Task_Read",
            "Template_Deny",
            "Template_LocalAdminFullAccess",
            "Conversation1000Participants",
            "ConversationDraft",
            "ConversationMessageModify",
            "ConversationObjects",
            "Bind_UnBindUserLicenseKeyTest",
            "BulkReadWriteDeleteTest",
            "TestAccountBasics",
            "TestAccountGroupEveryOneGroup",
            "TestAccountIsSystemAdministratorSign",
            "TestCacheClearAfterCreateAccount",
            "TestCommunicationChannelsADFS",
            "TestDateTimeLocal",
            "TestIncludesMemberUser",
            "TestTimezone",
            "TestTimezoneSpan",
            "TestWorkDays",
            "TestWorkDaysTZ",
            "UserTasksAssignee_Test",
            "AccountTemplateTest",
            "CartTest",
            "GetRoleWorkspaceItemsMTThread",
            "WritingIncomingAdapterMessageEvents",
            "CheckObjectHistory",
            "MultiValueRefInExpressionsTest",
            "TestMultiValueProperty",
            "TestObjectPropertyFullTextSearch",
            "AttributeTransferTest",
            "FormTransferTest",
            "MetadataTransferTest",
            "TestAttachmentViaLogContents",
            "TestDispose",
            "TestListMatch",
            "TestModelIMultipleInstances",
            "TestPCLImplies",
            "TestPCLIncludes",
            "TestQueryWithContext",
            "TestRealNestedTransaction_Think",
            "TestSequenceWithDecimal",
            "TestYAMLWriter",
            "TrackerTestDynamicModel"
        };
    }

    public static async Task Main()
    {
        var client = new HttpClient();
        var responseMessage = client.GetAsync("http://10.9.0.53/hosting/discovery").Result;
        var xml = await responseMessage.Content.ReadAsStringAsync();
        XmlSerializer serializer = new XmlSerializer(typeof(FastConsole.Wopidiscovery));
        using (StringReader reader = new StringReader(xml))
        {
            var test = (FastConsole.Wopidiscovery)serializer.Deserialize(reader);
        }
    }

    public static string[] GetRiderResults()
    {
        return new[] { "" };
    }

    public static string[] GetTeamcityTestResultsNames(IEnumerable<TeamcityTestsResult> tests)
    {
        return tests.Select(r => r.TestName.Split(' ').Last()).ToArray();
    }
    public static TeamcityTestsResult[] GetTeamcityTestResults(string csvPath)
    {
        using var reader = new StreamReader(csvPath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        return csv.GetRecords<TeamcityTestsResult>().ToArray();
    }
}