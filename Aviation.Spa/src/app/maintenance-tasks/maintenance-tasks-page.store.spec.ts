import { TestBed } from '@angular/core/testing';
import { of, throwError } from 'rxjs';
import { vi } from 'vitest';
import { MaintenanceTask } from '../models/maintenance-task.model';
import { Priority } from '../models/priority.model';
import { Status } from '../models/status.model';
import { MaintenanceTaskService } from '../services/maintenance-task.service';
import { MaintenanceTasksPageStore } from './maintenance-tasks-page.store';

describe('MaintenanceTasksPageStore', () => {
  let service: {
    getMaintenanceTasks: ReturnType<typeof vi.fn>;
    getStatuses: ReturnType<typeof vi.fn>;
    getPriorities: ReturnType<typeof vi.fn>;
  };

  const tasks: MaintenanceTask[] = [
    {
      id: 1,
      aircraftTailNumber: 'N123AB',
      aircraftRegistration: 'SX-DZA',
      component: 'Landing Gear',
      description: 'Inspect the landing gear assembly',
      statusId: 2,
      status: null,
      priorityId: 3,
      priority: null,
      createdAt: '2026-10-14T12:00:00Z',
      createdOn: '2026-10-14T12:00:00Z',
      createdBy: 'seed',
      lastUpdatedOn: '2026-10-14T12:00:00Z',
      lastUpdatedBy: 'seed',
      rowVersion: '',
    },
  ];

  const statuses: Status[] = [
    {
      id: 2,
      name: 'In Progress',
      createdOn: '2026-10-14T12:00:00Z',
      createdBy: 'seed',
      lastUpdatedOn: '2026-10-14T12:00:00Z',
      lastUpdatedBy: 'seed',
      rowVersion: '',
    },
  ];

  const priorities: Priority[] = [
    {
      id: 3,
      name: 'Critical',
      severity: 3,
      createdOn: '2026-10-14T12:00:00Z',
      createdBy: 'seed',
      lastUpdatedOn: '2026-10-14T12:00:00Z',
      lastUpdatedBy: 'seed',
      rowVersion: '',
    },
  ];

  beforeEach(() => {
    service = {
      getMaintenanceTasks: vi.fn(() => of([] as MaintenanceTask[])),
      getStatuses: vi.fn(() => of([] as Status[])),
      getPriorities: vi.fn(() => of([] as Priority[])),
    };

    TestBed.configureTestingModule({
      providers: [
        MaintenanceTasksPageStore,
        { provide: MaintenanceTaskService, useValue: service },
      ],
    });
  });

  it('loads tasks and their status and priority lookup data', () => {
    service.getMaintenanceTasks.mockReturnValue(of(tasks));
    service.getStatuses.mockReturnValue(of(statuses));
    service.getPriorities.mockReturnValue(of(priorities));

    const store = TestBed.inject(MaintenanceTasksPageStore);
    store.loadData();

    expect(store.tasks()).toEqual(tasks);
    expect(store.statuses()).toEqual(statuses);
    expect(store.priorities()).toEqual(priorities);
    expect(store.errorMessage()).toBeNull();
    expect(store.isLoading()).toBe(false);
  });

  it('treats a successful empty task response as an empty list', () => {
    const store = TestBed.inject(MaintenanceTasksPageStore);
    store.loadData();

    expect(store.tasks()).toEqual([]);
    expect(store.errorMessage()).toBeNull();
    expect(store.isLoading()).toBe(false);
  });

  it('exposes API errors instead of treating failed requests as empty results', () => {
    service.getStatuses.mockReturnValue(
      throwError(() => new Error('API unavailable')),
    );

    const store = TestBed.inject(MaintenanceTasksPageStore);
    store.loadData();

    expect(store.errorMessage()).toContain(
      'Maintenance tasks could not be loaded.',
    );
    expect(store.isLoading()).toBe(false);
  });
});
