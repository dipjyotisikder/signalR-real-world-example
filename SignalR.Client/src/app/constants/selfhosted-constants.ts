export const selfHostedConstants = {
  CREATE_CONVERSATION_ENDPOINT: 'messaging/conversations',
  GET_CONVERSATION_ENDPOINT: 'messaging/conversations',
  GET_AUDIENCE_ENDPOINT: (conversationId: number) =>
    `messaging/conversations/${conversationId}/audiences`,
  SIGNALR_ENDPOINT: 'signalR/applicationHub',

  GET_USERS_ENDPOINT: 'users',
  CREATE_USERS_ENDPOINT: 'users',
  UPDATE_USERS_ENDPOINT: 'users',
};
