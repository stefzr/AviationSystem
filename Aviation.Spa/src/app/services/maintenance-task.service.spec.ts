import { provideHttpClient } from '@angular/common/http';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { API_BASE_URL } from '../api.config';
import { MaintenanceTaskService } from './maintenance-task.service';

describe('MaintenanceTaskService', () => {
  let service: MaintenanceTaskService;
  let httpTestingController: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        MaintenanceTaskService,
        provideHttpClient(),
        provideHttpClientTesting(),
      ],
    });

    service = TestBed.inject(MaintenanceTaskService);
    httpTestingController = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpTestingController.verify();
  });

  it('requests maintenance tasks from the API', () => {
    service.getMaintenanceTasks().subscribe();

    const request = httpTestingController.expectOne(
      `${API_BASE_URL}/api/MaintenanceTasks`,
    );

    expect(request.request.method).toBe('GET');
    request.flush([]);
  });

  it('requests statuses from the API', () => {
    service.getStatuses().subscribe();

    const request = httpTestingController.expectOne(
      `${API_BASE_URL}/api/Statuses`,
    );

    expect(request.request.method).toBe('GET');
    request.flush([]);
  });

  it('requests priorities from the API', () => {
    service.getPriorities().subscribe();

    const request = httpTestingController.expectOne(
      `${API_BASE_URL}/api/Priorities`,
    );

    expect(request.request.method).toBe('GET');
    request.flush([]);
  });
});
