import { Injectable, inject } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {
  private loadingSubject = new BehaviorSubject<boolean>(false);
  public loading$: Observable<boolean> = this.loadingSubject.asObservable();

  private loadingMessageSubject = new BehaviorSubject<string>('Loading...');
  public loadingMessage$: Observable<string> = this.loadingMessageSubject.asObservable();

  show(message: string = 'Loading...'): void {
    this.loadingMessageSubject.next(message);
    this.loadingSubject.next(true);
  }

  hide(): void {
    this.loadingSubject.next(false);
  }

  isLoading(): boolean {
    return this.loadingSubject.value;
  }

  getMessage(): string {
    return this.loadingMessageSubject.value;
  }
}
