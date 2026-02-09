import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoadingService } from '../../services/loading.service';

@Component({
  selector: 'app-circular-loading',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './circular-loading.html',
  styleUrl: './circular-loading.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CircularLoadingComponent {
  private loadingService = inject(LoadingService);

  loading$ = this.loadingService.loading$;
  loadingMessage$ = this.loadingService.loadingMessage$;
}
