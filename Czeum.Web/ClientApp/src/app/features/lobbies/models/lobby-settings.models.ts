export interface LobbySettingsField {
  displayName: string;
  value: any;
}

export interface LobbySettings {
  [key: string]: LobbySettingsField;
}
