export interface LobbySettingsField {
  displayName: string;
  value: any;
}

export interface LobbySettings {
  [key: string]: LobbySettingsField;
}

export interface OriginalSettingsValues {
  [key: string]: any;
}
