import { Injectable, EventEmitter } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class RefreshService {
  refreshTriggered = new EventEmitter<void>();

  triggerRefresh(): void {
    this.refreshTriggered.emit();
  }
}
