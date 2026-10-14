import { Priority } from './priority.model';
import { Status } from './status.model';

export interface MaintenanceTask {
  id: number;
  aircraftTailNumber: string;
  aircraftRegistration: string;
  component: string;
  description: string;
  statusId: number;
  status: Status | null;
  priorityId: number;
  priority: Priority | null;
  createdAt: string;
  createdOn: string;
  createdBy: string;
  lastUpdatedOn: string;
  lastUpdatedBy: string;
  rowVersion: string;
}
