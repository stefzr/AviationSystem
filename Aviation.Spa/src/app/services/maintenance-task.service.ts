import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../api.config';
import { MaintenanceTask } from '../models/maintenance-task.model';
import { Priority } from '../models/priority.model';
import { Status } from '../models/status.model';

@Injectable({
  providedIn: 'root',
})
export class MaintenanceTaskService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${API_BASE_URL}/api`;

  getMaintenanceTasks(): Observable<MaintenanceTask[]> {
    return this.http.get<MaintenanceTask[]>(`${this.apiUrl}/MaintenanceTasks`);
  }

  getStatuses(): Observable<Status[]> {
    return this.http.get<Status[]>(`${this.apiUrl}/Statuses`);
  }

  getPriorities(): Observable<Priority[]> {
    return this.http.get<Priority[]>(`${this.apiUrl}/Priorities`);
  }
}
