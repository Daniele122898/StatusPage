import {Status} from './Status';

export interface ServiceStatus {
  identifier: string;
  status: Status;
  description?: string;
  error?: string;
  rtt: number;
  subEntities?: ServiceStatus[];
}

const isServiceCategory = (service: ServiceStatus): boolean => service.subEntities && service.subEntities.length > 0;

export {isServiceCategory};
