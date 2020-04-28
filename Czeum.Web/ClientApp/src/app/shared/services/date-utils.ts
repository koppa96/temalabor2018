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
