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

export const mockNotifications: Notification[] = [
  new Notification({
    id: '',
    icon: 'person_add',
    text: 'xyz123 barátkérelmet küldött.',
    data: 'friend-request-id',
    actions: {
      primary: {
        name: 'ELFOGAD',
        action: x => console.log(`Barátkérelem elfogadva: ${x}`)
      },
      secondary: {
        name: 'ELUTASÍT',
        action: x => console.log(`Barátkérelem elutasítva: ${x}`)
      }
    }
  }),
  new Notification({
    id: '',
    icon: 'mail_outline',
    text: '__TheLegend27__ meghívott, hogy csatlakozz a játékához.',
    data: 'lobby-id',
    actions: {
      primary: {
        name: 'ELFOGAD',
        action: x => console.log(`Barátkérelem elfogadva: ${x}`)
      },
      secondary: {
        name: 'ELUTASÍT',
        action: x => console.log(`Barátkérelem elutasítva: ${x}`)
      }
    }
  }),
  new Notification({
    id: '',
    icon: 'warning',
    text: 'user123 megbökött, mert régóta nem léptél!'
  }),
  new Notification({
    id: '',
    icon: 'person_add',
    text: 'gipsz.jakab elfogadta a barátkérelmed.'
  })
];
