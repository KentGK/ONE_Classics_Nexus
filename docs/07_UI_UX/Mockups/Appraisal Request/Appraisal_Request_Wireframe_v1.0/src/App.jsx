import { useMemo, useState } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faArrowRightFromBracket, faBarsProgress, faCalculator, faCalendarDays, faCar,
  faChartColumn, faCheck, faChevronDown, faChevronLeft, faChevronRight,
  faClipboardCheck, faClipboardList, faEllipsis, faFileLines, faFloppyDisk,
  faGear, faHouse, faMagnifyingGlass, faPaperPlane, faPlus, faRotate,
  faTrashCan, faTriangleExclamation, faUser, faUsers, faXmark,
} from "@fortawesome/free-solid-svg-icons";

const initialRequests = [
  { id: "AR-2026-00418", client: "Heritage Motors", vehicle: "1967 Ford Mustang", status: "Draft", requested: "1/26/2026", updated: "1/26 10:14 AM", contact: "Michael Reynolds", use: "Insurance coverage and agreed value.", date: "2026-02-01", value: "Fair Market Value", type: "Full Appraisal", priority: "Normal", scope: "Full appraisal with physical inspection, condition assessment, and market analysis. Focus on originality and documentation.", rowVersion: 3 },
  { id: "AR-2026-00417", client: "Smith Collection", vehicle: "1970 Chevrolet Chevelle SS", status: "Submitted", requested: "1/26/2026", updated: "1/26 9:02 AM", contact: "Dana Smith", use: "Estate planning", date: "2026-02-06", value: "Fair Market Value", type: "Desktop Appraisal", priority: "High", scope: "Documented market appraisal for estate planning.", rowVersion: 5 },
  { id: "AR-2026-00416", client: "Anderson Family", vehicle: "1985 Porsche 911 Carrera", status: "Under Review", requested: "1/25/2026", updated: "1/25 4:21 PM", contact: "Jill Anderson", use: "Charitable contribution", date: "2026-01-31", value: "Fair Market Value", type: "Full Appraisal", priority: "Normal", scope: "Inspect condition and verify supporting ownership documents.", rowVersion: 4 },
  { id: "AR-2026-00415", client: "Riverside Auto", vehicle: "1969 Dodge Charger R/T", status: "Accepted", requested: "1/24/2026", updated: "1/24 11:07 AM", contact: "Marco Ruiz", use: "Financing", date: "2026-02-10", value: "Market Value", type: "Full Appraisal", priority: "Normal", scope: "Lender-requested appraisal with inspection.", rowVersion: 7 },
  { id: "AR-2026-00414", client: "Thompson Estate", vehicle: "1972 Buick Riviera", status: "Converted", requested: "1/23/2026", updated: "1/26 8:15 AM", contact: "Anne Thompson", use: "Estate settlement", date: "2026-01-23", value: "Fair Market Value", type: "Full Appraisal", priority: "High", scope: "Estate settlement appraisal and supporting report.", rowVersion: 9 },
  { id: "AR-2026-00413", client: "Garcia Classics", vehicle: "1966 Chevrolet Corvette", status: "Accepted", requested: "1/22/2026", updated: "1/23 3:18 PM", contact: "Luis Garcia", use: "Insurance scheduling", date: "2026-02-03", value: "Replacement Cost", type: "Full Appraisal", priority: "Normal", scope: "Insurance appraisal with provenance review.", rowVersion: 6 },
  { id: "AR-2026-00412", client: "Wilson Automotive", vehicle: "1991 BMW M3", status: "Submitted", requested: "1/21/2026", updated: "1/22 1:11 PM", contact: "Chris Wilson", use: "Pre-purchase decision", date: "2026-02-08", value: "Market Value", type: "Desktop Appraisal", priority: "Normal", scope: "Pre-purchase condition and market review.", rowVersion: 2 },
  { id: "AR-2026-00411", client: "Taylor Collection", vehicle: "1968 Pontiac GTO", status: "Withdrawn", requested: "1/20/2026", updated: "1/20 5:40 PM", contact: "Megan Taylor", use: "Private sale", date: "2026-01-28", value: "Market Value", type: "Desktop Appraisal", priority: "Low", scope: "Request withdrawn by client.", rowVersion: 3 },
  { id: "AR-2026-00410", client: "Lee Investments", vehicle: "1973 Ferrari 365 GTB/4", status: "Under Review", requested: "1/20/2026", updated: "1/21 9:27 AM", contact: "Howard Lee", use: "Asset planning", date: "2026-02-12", value: "Fair Market Value", type: "Full Appraisal", priority: "High", scope: "Collector vehicle appraisal with provenance research.", rowVersion: 4 },
  { id: "AR-2026-00409", client: "Clark Motor Co.", vehicle: "1957 Chevrolet Bel Air", status: "Declined", requested: "1/19/2026", updated: "1/20 2:03 PM", contact: "Robin Clark", use: "Insurance", date: "2026-02-14", value: "Agreed Value", type: "Full Appraisal", priority: "Normal", scope: "Request declined after service-fit review.", rowVersion: 3 },
  { id: "AR-2026-00408", client: "Oak Street Garage", vehicle: "1965 Shelby Cobra", status: "Submitted", requested: "1/18/2026", updated: "1/19 12:04 PM", contact: "Avery Cole", use: "Financing", date: "2026-02-16", value: "Market Value", type: "Full Appraisal", priority: "High", scope: "Financing appraisal with authenticity review.", rowVersion: 2 },
  { id: "AR-2026-00407", client: "Bennett Family", vehicle: "1959 Cadillac Eldorado", status: "Draft", requested: "1/17/2026", updated: "1/18 8:44 AM", contact: "Paul Bennett", use: "Estate planning", date: "2026-02-18", value: "Fair Market Value", type: "Full Appraisal", priority: "Normal", scope: "Draft intake pending vehicle documentation.", rowVersion: 1 },
];

const navItems = [
  ["Dashboard", faHouse], ["Vehicles", faCar], ["Appraisal Requests", faClipboardList],
  ["Appraisals", faClipboardCheck], ["Inspections", faBarsProgress], ["Comparables", faChartColumn],
  ["Valuation", faCalculator], ["Reports", faFileLines], ["Admin", faGear],
];

const statusClass = (status) => `status status-${status.toLowerCase().replaceAll(" ", "-")}`;

function IconButton({ icon, children, className = "", ...props }) {
  return <button className={`button ${className}`} {...props}><FontAwesomeIcon icon={icon} />{children && <span>{children}</span>}</button>;
}

function Field({ label, required, children, className = "" }) {
  return <label className={`field ${className}`}><span className="field-label">{label}{required && <b aria-hidden="true"> *</b>}</span>{children}</label>;
}

function AppShell() {
  const [requests, setRequests] = useState(initialRequests);
  const [selectedId, setSelectedId] = useState(initialRequests[0].id);
  const [query, setQuery] = useState("");
  const [status, setStatus] = useState("All Statuses");
  const [client, setClient] = useState("All Clients");
  const [sort, setSort] = useState("Requested Date (Desc)");
  const [activeTab, setActiveTab] = useState("Request Information");
  const [moreOpen, setMoreOpen] = useState(false);
  const [toast, setToast] = useState("");
  const [dirty, setDirty] = useState(false);
  const selected = requests.find((item) => item.id === selectedId) ?? requests[0];

  const filtered = useMemo(() => {
    const value = query.trim().toLowerCase();
    return requests.filter((item) => (!value || `${item.id} ${item.client} ${item.vehicle}`.toLowerCase().includes(value)) && (status === "All Statuses" || item.status === status) && (client === "All Clients" || item.client === client));
  }, [requests, query, status, client]);

  const showToast = (message) => {
    setToast(message);
    window.clearTimeout(showToast.timer);
    showToast.timer = window.setTimeout(() => setToast(""), 2600);
  };

  const updateSelected = (field, value) => {
    if (["Accepted", "Converted", "Declined", "Withdrawn"].includes(selected.status)) return showToast("This lifecycle state requires an authorized amendment.");
    setRequests((items) => items.map((item) => item.id === selectedId ? { ...item, [field]: value } : item));
    setDirty(true);
  };

  const createRequest = () => {
    const created = { id: `AR-2026-${String(419 + requests.length).padStart(5, "0")}`, client: "Select client", vehicle: "Vehicle not selected", status: "Draft", requested: "Today", updated: "Just now", contact: "Select contact", use: "", date: "2026-02-01", value: "Fair Market Value", type: "Full Appraisal", priority: "Normal", scope: "", rowVersion: 1 };
    setRequests((items) => [created, ...items]); setSelectedId(created.id); setActiveTab("Request Information"); setDirty(true); showToast("New Draft request created.");
  };

  const save = () => { setRequests((items) => items.map((item) => item.id === selectedId ? { ...item, rowVersion: item.rowVersion + 1, updated: "Just now" } : item)); setDirty(false); showToast("Draft saved with a new row version."); };
  const submit = () => {
    if (!selected.client || selected.client === "Select client" || !selected.use || !selected.scope) return showToast("Submission blocked. Resolve required fields first.");
    setRequests((items) => items.map((item) => item.id === selectedId ? { ...item, status: "Submitted", rowVersion: item.rowVersion + 1 } : item)); setDirty(false); showToast("Request submitted and snapshots captured.");
  };
  const cancelRequest = () => { setRequests((items) => items.map((item) => item.id === selectedId ? { ...item, status: "Cancelled", rowVersion: item.rowVersion + 1 } : item)); setMoreOpen(false); showToast("Request cancelled with retained audit history."); };

  const clients = [...new Set(requests.map((item) => item.client))].filter((x) => x !== "Select client");
  const editable = !["Accepted", "Converted", "Declined", "Withdrawn", "Cancelled"].includes(selected.status);
  const warnings = selected.vehicle.includes("not selected") ? [["Vehicle", "A Vehicle has not been selected."], ["Request Information", "Complete the proposed intended use and scope."]] : [["Vehicle", "VIN has not been entered. Add VIN if available."], ["Notes and Attachments", "Consider adding supporting documentation."]];

  return <div className="app-shell">
    <header className="topbar">
      <div className="brand">ONE Classics Nexus</div>
      <nav className="top-links" aria-label="Product areas"><span>Appraisal Management</span><i /><span>Vehicles</span><i /><span>People</span><i /><span>Value</span></nav>
      <div className="user-box"><FontAwesomeIcon icon={faUser} /><div><strong>John Smith</strong><small>Intake Staff</small></div><FontAwesomeIcon icon={faChevronDown} /></div>
      <div className="window-controls" aria-hidden="true"><span>—</span><span>□</span><span>×</span></div>
    </header>

    <aside className="sidebar">{navItems.map(([label, icon]) => <button key={label} className={label === "Appraisal Requests" ? "nav-item active" : "nav-item"}><FontAwesomeIcon icon={icon} /><span>{label}</span></button>)}<button className="nav-item signout"><FontAwesomeIcon icon={faArrowRightFromBracket} /><span>Sign out</span></button></aside>

    <main className="workspace">
      <div className="workspace-title"><div><FontAwesomeIcon icon={faClipboardList} /><h1>Appraisal Requests</h1></div><span>Intake. Appraise. Deliver. With Confidence.</span></div>

      <section className="list-pane" aria-label="Appraisal Request list">
        <div className="list-toolbar"><IconButton icon={faPlus} className="primary new-button" onClick={createRequest}>New Request</IconButton><IconButton icon={faRotate} className="secondary refresh" onClick={() => showToast("Request list refreshed.")}>Refresh</IconButton></div>
        <div className="search-box"><FontAwesomeIcon icon={faMagnifyingGlass} /><input aria-label="Search requests" value={query} onChange={(e) => setQuery(e.target.value)} placeholder="Search requests, client, or vehicle..." />{query && <button aria-label="Clear search" onClick={() => setQuery("")}><FontAwesomeIcon icon={faXmark} /></button>}</div>
        <div className="filter-grid">
          <Field label="Status"><select value={status} onChange={(e) => setStatus(e.target.value)}><option>All Statuses</option>{[...new Set(requests.map((r) => r.status))].map((value) => <option key={value}>{value}</option>)}</select></Field>
          <Field label="Client"><select value={client} onChange={(e) => setClient(e.target.value)}><option>All Clients</option>{clients.map((value) => <option key={value}>{value}</option>)}</select></Field>
          <Field label="Date Range"><select defaultValue="Requested Date"><option>Requested Date</option><option>Updated Date</option></select></Field>
          <div className="date-range"><span>1/1/2026</span><b>–</b><span>1/31/2026</span><FontAwesomeIcon icon={faCalendarDays} /></div>
        </div>
        <div className="list-summary"><strong>{filtered.length} requests found</strong><label>Sort by <select value={sort} onChange={(e) => setSort(e.target.value)}><option>Requested Date (Desc)</option><option>Updated Date (Desc)</option><option>Client (A–Z)</option></select></label></div>
        <div className="table-wrap"><table><thead><tr><th>Request #</th><th>Client</th><th>Vehicle</th><th>Status</th><th>Requested</th><th>Updated</th></tr></thead><tbody>{filtered.slice(0, 10).map((item) => <tr key={item.id} className={item.id === selectedId ? "selected" : ""} onClick={() => { setSelectedId(item.id); setDirty(false); }} tabIndex="0" onKeyDown={(e) => e.key === "Enter" && setSelectedId(item.id)}><td className="request-id">{item.id}</td><td>{item.client}</td><td className="vehicle-cell">{item.vehicle}</td><td><span className={statusClass(item.status)}>{item.status}</span></td><td>{item.requested}</td><td>{item.updated}</td></tr>)}</tbody></table>{filtered.length === 0 && <div className="empty-state"><FontAwesomeIcon icon={faMagnifyingGlass} /><strong>No requests found</strong><span>Adjust your filters or create a new request.</span></div>}</div>
        <div className="pagination"><div><button disabled><FontAwesomeIcon icon={faChevronLeft} /></button><button className="current">1</button><button>2</button><button><FontAwesomeIcon icon={faChevronRight} /></button></div><span>Showing 1–{Math.min(10, filtered.length)} of {filtered.length}</span></div>
      </section>

      <section className="detail-pane" aria-label="Selected Appraisal Request">
        <div className="detail-header">
          <div className="request-heading"><h2>{selected.id}</h2><span className={statusClass(selected.status)}>{selected.status}</span>{dirty && <span className="unsaved">Unsaved changes</span>}</div>
          <div className="detail-actions"><IconButton icon={faFloppyDisk} className="primary" aria-label="Save" onClick={save} disabled={!editable}>Save</IconButton><IconButton icon={faCheck} className="secondary" aria-label="Validate" onClick={() => showToast(`${warnings.length} warnings found. No blocking issues.`)}>Validate</IconButton><IconButton icon={faPaperPlane} className="secondary" aria-label="Submit" onClick={submit} disabled={selected.status !== "Draft"}>Submit</IconButton><div className="more-wrap"><IconButton icon={faChevronDown} className="secondary more" aria-label="More Actions" onClick={() => setMoreOpen(!moreOpen)}>More Actions</IconButton>{moreOpen && <div className="more-menu"><button onClick={cancelRequest}>Cancel request</button><button onClick={() => showToast("Withdraw requires requester confirmation.")}>Withdraw request</button><button className="danger" disabled title="Only an eligible unreferenced Draft may be purged"><FontAwesomeIcon icon={faTrashCan} /> Delete request</button></div>}</div></div>
          <div className="audit-line">Row Version: {selected.rowVersion}<i /> Created: 1/26/2026 9:12 AM <i /> Updated: {selected.updated}</div>
        </div>

        <div className="tabs" role="tablist">{["Request Information", "Client and Intended Users", "Vehicle", "Notes and Attachments", "History"].map((tab) => <button key={tab} role="tab" aria-selected={activeTab === tab} className={activeTab === tab ? "active" : ""} onClick={() => setActiveTab(tab)}>{tab}</button>)}</div>

        <div className="tab-content">
          {activeTab === "Request Information" && <div className="request-form">
            <div className="form-column">
              <Field label="Client" required><div className="field-with-action"><select value={selected.client} disabled={!editable} onChange={(e) => updateSelected("client", e.target.value)}><option>Select client</option>{clients.map((value) => <option key={value}>{value}</option>)}</select><button aria-label="Open client lookup"><FontAwesomeIcon icon={faEllipsis} /></button></div></Field>
              <Field label="Primary Contact" required><div className="field-with-action"><select value={selected.contact} disabled={!editable} onChange={(e) => updateSelected("contact", e.target.value)}><option>Select contact</option><option>Michael Reynolds</option><option>Dana Smith</option><option>Jill Anderson</option></select><button aria-label="Open contact lookup"><FontAwesomeIcon icon={faEllipsis} /></button></div></Field>
              <Field label="Proposed Intended Use" required><textarea disabled={!editable} value={selected.use} onChange={(e) => updateSelected("use", e.target.value)} /></Field>
            </div>
            <div className="form-column right-column">
              <Field label="Effective Date" required><input type="date" disabled={!editable} value={selected.date} onChange={(e) => updateSelected("date", e.target.value)} /></Field>
              <Field label="Value Definition" required><select disabled={!editable} value={selected.value} onChange={(e) => updateSelected("value", e.target.value)}><option>Fair Market Value</option><option>Market Value</option><option>Replacement Cost</option><option>Agreed Value</option></select></Field>
              <Field label="Appraisal Type" required><select disabled={!editable} value={selected.type} onChange={(e) => updateSelected("type", e.target.value)}><option>Full Appraisal</option><option>Desktop Appraisal</option><option>Inspection Only</option></select></Field>
              <Field label="Priority" required><select disabled={!editable} value={selected.priority} onChange={(e) => updateSelected("priority", e.target.value)}><option>Low</option><option>Normal</option><option>High</option></select></Field>
            </div>
            <Field label="Scope Summary" required className="scope-field"><textarea disabled={!editable} value={selected.scope} onChange={(e) => updateSelected("scope", e.target.value)} /></Field>
          </div>}
          {activeTab === "Client and Intended Users" && <div className="placeholder-panel"><FontAwesomeIcon icon={faUsers} /><h3>Client and Intended Users</h3><p>Manage the engaging Client, selected Client Contact, and request-owned Intended User snapshots.</p><button className="button primary"><FontAwesomeIcon icon={faPlus} /> Add Intended User</button></div>}
          {activeTab === "Vehicle" && <div className="placeholder-panel"><FontAwesomeIcon icon={faCar} /><h3>{selected.vehicle}</h3><p>Search for an existing Vehicle or capture preliminary identifying facts without overwriting Vehicle master data.</p><button className="button secondary"><FontAwesomeIcon icon={faMagnifyingGlass} /> Search Vehicles</button></div>}
          {activeTab === "Notes and Attachments" && <div className="placeholder-panel"><FontAwesomeIcon icon={faFileLines} /><h3>Notes and Attachments</h3><p>Keep intake notes and supporting documents linked to the request workfile.</p><button className="button primary"><FontAwesomeIcon icon={faPlus} /> Add Attachment</button></div>}
          {activeTab === "History" && <div className="history-panel"><h3>Request History</h3><div><b>1/26/2026 10:14 AM</b><span>Draft updated by John Smith · Row Version {selected.rowVersion}</span></div><div><b>1/26/2026 9:12 AM</b><span>Appraisal Request created · Status Draft</span></div></div>}
        </div>

        <div className="validation-panel"><div className="validation-title"><div><span className="success-icon"><FontAwesomeIcon icon={faCheck} /></span><h3>Validation Results</h3></div><span><b>0</b> blocking issues <i /> <strong>{warnings.length} warnings</strong></span></div><table><thead><tr><th>Severity</th><th>Field / Section</th><th>Message</th></tr></thead><tbody>{warnings.map(([field, message]) => <tr key={field}><td><FontAwesomeIcon icon={faTriangleExclamation} /> Warning</td><td>{field}</td><td>{message}</td></tr>)}</tbody></table></div>
        <div className="detail-footer"><IconButton icon={faTrashCan} className="delete" disabled>Delete Request</IconButton><button className="button secondary" onClick={() => showToast("No changes were discarded.")}>Cancel</button></div>
      </section>
    </main>
    {toast && <div className="toast" role="status"><FontAwesomeIcon icon={faCheck} />{toast}</div>}
  </div>;
}

export function App() { return <AppShell />; }
