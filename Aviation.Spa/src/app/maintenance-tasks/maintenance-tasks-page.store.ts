import { Injectable, inject, signal } from '@angular/core';
import { forkJoin } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { MaintenanceTask } from '../models/maintenance-task.model';
import { Priority } from '../models/priority.model';
import { Status } from '../models/status.model';
import { MaintenanceTaskService } from '../services/maintenance-task.service';

@Injectable()
export class MaintenanceTasksPageStore {
  private readonly maintenanceTaskService = inject(MaintenanceTaskService);

  readonly tasks = signal<MaintenanceTask[]>([]);
  readonly statuses = signal<Status[]>([]);
  readonly priorities = signal<Priority[]>([]);
  readonly isLoading = signal(true);
  readonly errorMessage = signal<string | null>(null);
  readonly allowedPageSizes = [10, 20, 50];

  loadData(): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);

    forkJoin({
      tasks: this.maintenanceTaskService.getMaintenanceTasks(),
      statuses: this.maintenanceTaskService.getStatuses(),
      priorities: this.maintenanceTaskService.getPriorities(),
    })
      .pipe(finalize(() => this.isLoading.set(false)))
      .subscribe({
        next: ({ tasks, statuses, priorities }) => {
          this.tasks.set(tasks);
          this.statuses.set(statuses);
          this.priorities.set(priorities);
        },
        error: () => {
          this.errorMessage.set(
            'Maintenance tasks could not be loaded. Check the API connection and try again.',
          );
        },
      });
  }
}
