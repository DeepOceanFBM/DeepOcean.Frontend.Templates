import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

export interface Client {
  Id: number;
  Name: string;
  Phone: string;    // actual field in Model/Client.cs is Phone (not Phone2)
}

// Matches DeepOcean SDK ServiceResponseModel<T>
interface ServiceResponse<T> {
  Success: boolean;
  Message: string;
  CodeStatus: number;
  Data: T | null;
}

// Local server base URL — matches miniBackend routing
const LOCAL_API = 'http://localhost:18080/g.d.f.b.m';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  encapsulation: ViewEncapsulation.None
})
export class AppComponent implements OnInit {
  title = 'DeepOcean.Frontend.Template';

  // ── State ──────────────────────────────────────
  clients: Client[] = [];
  isLoading = false;
  isSubmitting = false;
  errorMessage = '';
  successMessage = '';
  showModal = false;
  isEditing = false;
  deleteConfirmId: number | null = null;
  searchQuery = '';

  // ── Form model ─────────────────────────────────
  formData: Partial<Client> = { Name: '', Phone: '' };

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.loadClients();
  }

  // ── Get (calls: Clinets.Get) ──
  // Endpoint: GET http://localhost:18080/g.d.f.b.m/LogicProject/LogicProject.Clinets/Get?Index=1&pagezie=100
  loadClients(): void {
    this.isLoading = true;
    this.errorMessage = '';

    const params = new HttpParams()
      .set('Index', '0')
      .set('pagezie', '100');

    this.http.get<ServiceResponse<Client[]>>(`${LOCAL_API}/LogicProject/LogicProject.Clinets/Get`, { params })
      .subscribe({
        next: (res) => {
          if (res && res.Success && res.Data) {
            this.clients = res.Data;
          } else {
            this.errorMessage = res?.Message || 'Failed to load clients.';
          }
          this.isLoading = false;
        },
        error: (err) => {
          this.errorMessage = `Connection error: ${err.status === 0 ? 'Local server is not running. Start the DeepOcean Platform Client first.' : err.message}`;
          this.isLoading = false;
        }
      });
  }

  // ── Add / Update (calls: Clinets.Add) ──
  // Endpoint: POST http://localhost:18080/g.d.f.b.m/LogicProject/LogicProject.Clinets/Add
  submitForm(): void {
    if (!this.formData.Name?.trim()) {
      this.errorMessage = 'Client Name is required.';
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = '';

    const endpoint = `${LOCAL_API}/LogicProject/LogicProject.Clinets/Add`;

    // The backend expects a string parameter 'ClinetJson', so we stringify the object.
    // Some routers might expect the string itself to be JSON serialized (a quoted string), 
    // or just the raw JSON object. We will send the stringified JSON with application/json header.
    const payload = JSON.stringify(this.formData);

    this.http.post<ServiceResponse<boolean>>(endpoint, payload, {
      headers: { 'Content-Type': 'application/json' }
    })
      .subscribe({
        next: (res) => {
          if (res && res.Success) {
            this.showSuccess(this.isEditing ? 'Client updated successfully!' : 'Client added successfully!');
            this.closeModal();
            this.loadClients();
          } else {
            this.errorMessage = res?.Message || 'Failed to save client.';
          }
          this.isSubmitting = false;
        },
        error: (err) => {
          this.errorMessage = `Save error: ${err.message}`;
          this.isSubmitting = false;
        }
      });
  }

  // ── Delete ─────────────────────────────────────
  // Endpoint: GET http://localhost:18080/g.d.f.b.m/LogicProject/LogicProject.Clinets/Delete?id={id}
  confirmDelete(id: number): void {
    this.deleteConfirmId = id;
  }

  deleteClient(): void {
    if (this.deleteConfirmId == null) return;
    const id = this.deleteConfirmId;

    const params = new HttpParams().set('id', id.toString());
    const endpoint = `${LOCAL_API}/LogicProject/LogicProject.Clinets/Delete`;

    this.http.get<ServiceResponse<boolean>>(endpoint, { params })
      .subscribe({
        next: (res) => {
          if (res && res.Success) {
            this.showSuccess('Client deleted successfully!');
            this.loadClients();
          } else {
            this.errorMessage = res?.Message || 'Failed to delete.';
          }
          this.deleteConfirmId = null;
        },
        error: (err) => {
          this.errorMessage = `Delete error: ${err.message}`;
          this.deleteConfirmId = null;
        }
      });
  }

  cancelDelete(): void {
    this.deleteConfirmId = null;
  }

  // ── Modal helpers ──────────────────────────────
  openAddModal(): void {
    this.isEditing = false;
    this.formData = { Name: '', Phone: '' };
    this.errorMessage = '';
    this.showModal = true;
  }

  openEditModal(client: Client): void {
    this.isEditing = true;
    this.formData = { ...client };
    this.errorMessage = '';
    this.showModal = true;
  }

  closeModal(): void {
    this.showModal = false;
    this.errorMessage = '';
  }

  // ── Search / Filter ────────────────────────────
  get filteredClients(): Client[] {
    const q = this.searchQuery.toLowerCase();
    if (!q) return this.clients;
    return this.clients.filter(c =>
      c.Name.toLowerCase().includes(q) ||
      (c.Phone || '').toLowerCase().includes(q)
    );
  }

  // ── Utils ──────────────────────────────────────
  private showSuccess(msg: string): void {
    this.successMessage = msg;
    setTimeout(() => this.successMessage = '', 3000);
  }

  trackById(_: number, client: Client): number {
    return client.Id;
  }
}
