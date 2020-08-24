export interface StatusConfig {
  identifier: string;
  description?: string;
  healthEndpoint: string;
  subEntities?: StatusConfig[];
  enabled: boolean;
  isCategory: boolean;
}
