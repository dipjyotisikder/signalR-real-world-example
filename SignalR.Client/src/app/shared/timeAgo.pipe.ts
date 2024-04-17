import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'timeAgo',
})
export class TimeAgoPipe implements PipeTransform {
  transform(value: string): string {
    const time = new Date(value);
    const now = new Date();
    const seconds = Math.floor((now.getTime() - time.getTime()) / 1000);

    if (seconds < 60) {
      return 'just now';
    } else if (seconds < 120) {
      return 'a minute ago';
    } else if (seconds < 3600) {
      return Math.floor(seconds / 60) + ' minutes ago';
    } else if (seconds < 7200) {
      return 'an hour ago';
    } else if (seconds < 86400) {
      return Math.floor(seconds / 3600) + ' hours ago';
    } else {
      return time.toLocaleString();
    }
  }
}
