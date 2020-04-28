export interface Ordering<T> {
  displayName: string;
  comparator: (left: T, right: T) => number;
}
