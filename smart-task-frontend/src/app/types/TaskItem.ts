export interface TaskItem {
    id: string;
    title: string;
    description: string;
    createdAt: string;
    status: 'Open' | 'InProgress' | 'Done';
    priority: 'Low' | 'Medium' | 'High';
}