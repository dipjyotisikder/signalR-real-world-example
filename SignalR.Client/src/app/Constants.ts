export const HubConstants = {
  JOIN_GROUP_HUB_METHOD: 'JoinGroup',
  BROADCAST_HUB_METHOD: 'Broadcast',

  NOTIFICATION_CREATED_HUB_EVENT: 'NotificationCreated',
  CONNECTED_CLIENT_UPDATED_HUB_EVENT: 'ConnectedClientUpdated',
  EXCEPTION_OCCURRED_HUB_EVENT: 'ExceptionOccurred',

  serverEvents: {
    USER_IS_ONLINE: 'UserIsOnLine',
    MESSAGE_IS_CREATED: 'MessageIsCreated',
  },
};

export const MessageConstants = {
  CONNECTION_FAILED: 'Could not connect!',
  PLEASE_CONNECT_ALERT: 'Please connect first!',
};
