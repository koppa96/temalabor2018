export function toLocalDate(utcDate: string | Date): Date {
  const date = new Date(utcDate);
  date.setUTCFullYear(date.getFullYear());
  date.setUTCMonth(date.getMonth());
  date.setUTCDate(date.getDate());
  date.setUTCHours(date.getHours());
  date.setUTCMinutes(date.getMinutes());
  date.setUTCSeconds(date.getSeconds());
  date.setUTCMilliseconds(date.getMilliseconds());
  return date;
}

export function getLastOnlineText(lastOnline: Date): string {
  const now = new Date();
  if (now.getTime() - lastOnline.getTime() < 86400000) {
    const elapsedMinutes = Math.floor((now.getTime() - lastOnline.getTime()) / 60000);
    if (elapsedMinutes < 10) {
      return 'néhány perce';
    } else if (elapsedMinutes < 60) {
      return `${elapsedMinutes} perce`;
    } else if (elapsedMinutes / 60 < 24) {
      return `${Math.floor(elapsedMinutes / 60)} órája`;
    }
  } else {
    return `${lastOnline.getFullYear()}. ${lastOnline.getMonth() + 1 < 10 ? `0${lastOnline.getMonth() + 1}` : lastOnline.getMonth() + 1}. ${lastOnline.getDate() < 10 ? `0${lastOnline.getDate()}` : lastOnline.getDate() }.`;
  }
}
