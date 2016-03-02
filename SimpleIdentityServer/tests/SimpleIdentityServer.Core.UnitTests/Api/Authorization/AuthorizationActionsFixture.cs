﻿using System.Collections.Generic;
using System.Security.Principal;
using Moq;
using SimpleIdentityServer.Core.Api.Authorization;
using SimpleIdentityServer.Core.Api.Authorization.Actions;
using SimpleIdentityServer.Core.Common.Extensions;
using SimpleIdentityServer.Core.Helpers;
using SimpleIdentityServer.Core.Models;
using SimpleIdentityServer.Core.Parameters;
using SimpleIdentityServer.Core.Results;
using SimpleIdentityServer.Core.Validators;
using SimpleIdentityServer.Logging;
using Xunit;

namespace SimpleIdentityServer.Core.UnitTests.Api.Authorization
{
    public sealed class AuthorizationActionsFixture : BaseFixture
    {
        private Mock<IGetAuthorizationCodeOperation> _getAuthorizationCodeOperationFake;

        private Mock<IGetTokenViaImplicitWorkflowOperation> _getTokenViaImplicitWorkflowOperationFake;

        private Mock<IGetAuthorizationCodeAndTokenViaHybridWorkflowOperation>
            _getAuthorizationCodeAndTokenViaHybridWorkflowOperationFake;

        private Mock<IAuthorizationCodeGrantTypeParameterAuthEdpValidator> _authorizationCodeGrantTypeParameterAuthEdpValidatorFake;

        private Mock<IParameterParserHelper> _parameterParserHelperFake;

        private Mock<ISimpleIdentityServerEventSource> _simpleIdentityServerEventSourceFake;

        private Mock<IAuthorizationFlowHelper> _authorizationFlowHelperFake;

        private IAuthorizationActions _authorizationActions;

        [Fact]
        public void When_Starting_Implicit_Authorization_Process_Then_Event_Is_Started_And_Ended()
        {
            // ARRANGE
            InitializeFakeObjects();
            var actionResult = new ActionResult
            {
                Type = TypeActionResult.RedirectToAction,
                RedirectInstruction = new RedirectInstruction
                {
                    Action = IdentityServerEndPoints.ConsentIndex
                }
            };

            _parameterParserHelperFake.Setup(p => p.ParseResponseType(It.IsAny<string>()))
                .Returns(new List<ResponseType>
                {
                    ResponseType.id_token
                });
            _getTokenViaImplicitWorkflowOperationFake.Setup(g => g.Execute(It.IsAny<AuthorizationParameter>(),
                It.IsAny<IPrincipal>())).Returns(actionResult);
            _authorizationFlowHelperFake.Setup(a => a.GetAuthorizationFlow(It.IsAny<ICollection<ResponseType>>(),
                It.IsAny<string>()))
                .Returns(AuthorizationFlow.ImplicitFlow);

            const string clientId = "clientId";
            const string responseType = "id_token";
            const string scope = "openid";
            const string actionType = "RedirectToAction";
            const string controllerAction = "ConsentIndex";

            var authorizationParameter = new AuthorizationParameter
            {
                ClientId = clientId,
                ResponseType = responseType,
                Scope = scope,
                Claims = null
            };
            var serializedParameter = actionResult.RedirectInstruction.Parameters.SerializeWithJavascript();

            // ACT
            _authorizationActions.GetAuthorization(authorizationParameter, null);

            // ASSERTS
            _simpleIdentityServerEventSourceFake.Verify(s => s.StartAuthorization(clientId, responseType, scope, string.Empty));
            _simpleIdentityServerEventSourceFake.Verify(s => s.EndAuthorization(actionType, controllerAction, serializedParameter));
        }

        [Fact]
        public void When_Starting_AuthorizationCode_Authorization_Process_Then_Event_Is_Started_And_Ended()
        {
            // ARRANGE
            InitializeFakeObjects();
            var actionResult = new ActionResult
            {
                Type = TypeActionResult.RedirectToAction,
                RedirectInstruction = new RedirectInstruction
                {
                    Action = IdentityServerEndPoints.ConsentIndex
                }
            };

            _parameterParserHelperFake.Setup(p => p.ParseResponseType(It.IsAny<string>()))
                .Returns(new List<ResponseType>
                {
                    ResponseType.id_token
                });
            _getAuthorizationCodeOperationFake.Setup(g => g.Execute(It.IsAny<AuthorizationParameter>(),
                It.IsAny<IPrincipal>())).Returns(actionResult);
            _authorizationFlowHelperFake.Setup(a => a.GetAuthorizationFlow(It.IsAny<ICollection<ResponseType>>(),
                It.IsAny<string>()))
                .Returns(AuthorizationFlow.AuthorizationCodeFlow);

            const string clientId = "clientId";
            const string responseType = "id_token";
            const string scope = "openid";
            const string actionType = "RedirectToAction";
            const string controllerAction = "ConsentIndex";

            var authorizationParameter = new AuthorizationParameter
            {
                ClientId = clientId,
                ResponseType = responseType,
                Scope = scope,
                Claims = null
            };
            var serializedParameter = actionResult.RedirectInstruction.Parameters.SerializeWithJavascript();

            // ACT
            _authorizationActions.GetAuthorization(authorizationParameter, null);

            // ASSERTS
            _simpleIdentityServerEventSourceFake.Verify(s => s.StartAuthorization(clientId, responseType, scope, string.Empty));
            _simpleIdentityServerEventSourceFake.Verify(s => s.EndAuthorization(actionType, controllerAction, serializedParameter));
        }

        [Fact]
        public void When_Starting_Hybrid_Authorization_Process_Then_Event_Is_Started_And_Ended()
        {
            // ARRANGE
            InitializeFakeObjects();
            var actionResult = new ActionResult
            {
                Type = TypeActionResult.RedirectToAction,
                RedirectInstruction = new RedirectInstruction
                {
                    Action = IdentityServerEndPoints.ConsentIndex
                }
            };

            _parameterParserHelperFake.Setup(p => p.ParseResponseType(It.IsAny<string>()))
                .Returns(new List<ResponseType>
                {
                    ResponseType.id_token
                });
            _getAuthorizationCodeAndTokenViaHybridWorkflowOperationFake.Setup(g => g.Execute(It.IsAny<AuthorizationParameter>(),
                It.IsAny<IPrincipal>())).Returns(actionResult);
            _authorizationFlowHelperFake.Setup(a => a.GetAuthorizationFlow(It.IsAny<ICollection<ResponseType>>(),
                It.IsAny<string>()))
                .Returns(AuthorizationFlow.HybridFlow);

            const string clientId = "clientId";
            const string responseType = "id_token";
            const string scope = "openid";
            const string actionType = "RedirectToAction";
            const string controllerAction = "ConsentIndex";

            var authorizationParameter = new AuthorizationParameter
            {
                ClientId = clientId,
                ResponseType = responseType,
                Scope = scope,
                Claims = null
            };
            var serializedParameter = actionResult.RedirectInstruction.Parameters.SerializeWithJavascript();

            // ACT
            _authorizationActions.GetAuthorization(authorizationParameter, null);

            // ASSERTS
            _simpleIdentityServerEventSourceFake.Verify(s => s.StartAuthorization(clientId, responseType, scope, string.Empty));
            _simpleIdentityServerEventSourceFake.Verify(s => s.EndAuthorization(actionType, controllerAction, serializedParameter));
        }

        private void InitializeFakeObjects()
        {
            _getAuthorizationCodeOperationFake = new Mock<IGetAuthorizationCodeOperation>();
            _getTokenViaImplicitWorkflowOperationFake = new Mock<IGetTokenViaImplicitWorkflowOperation>();
            _getAuthorizationCodeAndTokenViaHybridWorkflowOperationFake = new Mock<IGetAuthorizationCodeAndTokenViaHybridWorkflowOperation>();
            _authorizationCodeGrantTypeParameterAuthEdpValidatorFake =
                new Mock<IAuthorizationCodeGrantTypeParameterAuthEdpValidator>();
            _parameterParserHelperFake = new Mock<IParameterParserHelper>();
            _simpleIdentityServerEventSourceFake = new Mock<ISimpleIdentityServerEventSource>();
            _authorizationFlowHelperFake = new Mock<IAuthorizationFlowHelper>();
            _authorizationActions = new AuthorizationActions(
                _getAuthorizationCodeOperationFake.Object,
                _getTokenViaImplicitWorkflowOperationFake.Object,
                _getAuthorizationCodeAndTokenViaHybridWorkflowOperationFake.Object,
                _authorizationCodeGrantTypeParameterAuthEdpValidatorFake.Object,
                _parameterParserHelperFake.Object,
                _simpleIdentityServerEventSourceFake.Object,
                _authorizationFlowHelperFake.Object);
        }
    }
}