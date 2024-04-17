export const selfHostedConstants = {
  CREATE_CONVERSATION_ENDPOINT: 'messaging/conversations',

  GET_CONVERSATIONS_ENDPOINT: 'messaging/conversations',

  GET_AUDIENCE_ENDPOINT: (conversationId: number) =>
    `messaging/conversations/${conversationId}/audiences`,

  GET_MESSAGES_ENDPOINT: (conversationId: number) =>
    `messaging/conversations/${conversationId}/messages`,

  CREATE_MESSAGE_ENDPOINT: (conversationId: number) =>
    `messaging/conversations/${conversationId}/messages`,

  GET_AUDIENCES_ENDPOINT: (conversationId: number) =>
    `messaging/conversations/${conversationId}/audiences`,

  GET_USERS_ENDPOINT: 'users',
  CREATE_USERS_ENDPOINT: 'users',
  UPDATE_USERS_ENDPOINT: 'users',

  REFRESH_TOKEN_ENDPOINT: 'users/refreshToken',

  SIGNALR_ENDPOINT: 'signalR/applicationHub',
};
