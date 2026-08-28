import { Pipe, PipeTransform } from '@angular/core';
import { Client } from './app.component';

@Pipe({ name: 'phoneCount' })
export class PhoneCountPipe implements PipeTransform {
  transform(clients: Client[]): number {
    return clients.filter(c => c.Phone && c.Phone.trim().length > 0).length;
  }
}
