export interface NotificationAction {
  name: string;
  action: (param: any) => any;
}

export interface NotificationActions {
  primary: NotificationAction;
  secondary?: NotificationAction;
}

export interface INotification {
  id: string;
  icon: string;
  text: string;
  data?: any;
  actions?: NotificationActions;
}

export class Notification implements INotification {
  id: string;
  icon: string;
  text: string;
  data?: any;
  actions?: NotificationActions;

  constructor(content: INotification) {
    this.id = content.id;
    this.icon = content.icon;
    this.text = content.text;
    this.data = content.data;
    this.actions = content.actions;
  }

  invokePrimaryAction() {
    if (this.actions) {
      this.actions.primary.action(this.data);
    }
  }

  invokeSecondaryAction() {
    if (this.actions && this.actions.secondary) {
      this.actions.secondary.action(this.data);
    }
  }
}
