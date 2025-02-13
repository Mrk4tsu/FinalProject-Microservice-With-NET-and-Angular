import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  standalone: true,
  name: 'fistKey'
})
export class FistKeyPipe implements PipeTransform {

  transform(value: any): string | null {
    const keys = Object.keys(value);
    if (keys.length > 0) {
      return keys[0];
    }
    return null;
  }
}
