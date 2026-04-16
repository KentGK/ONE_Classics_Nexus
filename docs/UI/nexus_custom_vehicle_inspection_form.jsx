import React, { useMemo, useState } from "react";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Textarea } from "@/components/ui/textarea";
import { Checkbox } from "@/components/ui/checkbox";
import { Label } from "@/components/ui/label";
import { Badge } from "@/components/ui/badge";
import { Progress } from "@/components/ui/progress";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { Accordion, AccordionContent, AccordionItem, AccordionTrigger } from "@/components/ui/accordion";
import { Camera, Car, ClipboardList, FileText, AlertTriangle, Gauge, ShieldCheck, Wrench, Upload, Save, CheckCircle2 } from "lucide-react";
import { motion } from "framer-motion";

const systems = [
  {
    key: "identity",
    title: "1. Vehicle Identity & Build Profile",
    icon: Car,
    fields: [
      { label: "Year", type: "text", placeholder: "1970" },
      { label: "Make", type: "text", placeholder: "Chevrolet" },
      { label: "Model", type: "text", placeholder: "C10" },
      { label: "Trim / Series", type: "text", placeholder: "Custom / Deluxe / Base" },
      { label: "Body Style", type: "text", placeholder: "Pickup / Coupe / Convertible / Wagon" },
      { label: "VIN / Serial Number", type: "text", placeholder: "Enter VIN or serial" },
      { label: "Builder / Shop", type: "text", placeholder: "Optional" },
      { label: "Build Type", type: "select", options: ["Custom", "Restomod", "Pro-Touring", "Street Rod", "Show Vehicle", "Off-Road Custom", "Other"] },
    ],
    checklist: [
      "VIN/serial verified",
      "Build theme documented",
      "Major modifications identified",
      "Ownership / provenance notes captured",
    ],
  },
  {
    key: "body",
    title: "2. Body, Paint & Exterior Finish",
    icon: ShieldCheck,
    fields: [
      { label: "Exterior Condition Score", type: "score" },
      { label: "Paint Quality", type: "select", options: ["Show", "Excellent Driver", "Good Driver", "Fair", "Needs Refinish"] },
      { label: "Panel Fit", type: "select", options: ["Excellent", "Good", "Fair", "Poor"] },
      { label: "Rust / Corrosion", type: "select", options: ["None Observed", "Minor", "Moderate", "Severe"] },
    ],
    checklist: [
      "Body lines and panel gaps reviewed",
      "Paint depth / finish consistency noted",
      "Custom fabrication quality reviewed",
      "Rust, bubbling, cracks, chips, and repairs noted",
    ],
  },
  {
    key: "glass",
    title: "3. Glass, Trim, Lighting & Exterior Details",
    icon: Camera,
    fields: [
      { label: "Glass / Trim Score", type: "score" },
      { label: "Glass Condition", type: "select", options: ["Excellent", "Good", "Fair", "Poor"] },
      { label: "Lighting Function", type: "select", options: ["All Working", "Most Working", "Partial", "Unknown"] },
    ],
    checklist: [
      "Windshield and side glass checked",
      "Custom trim / chrome quality reviewed",
      "Headlamps, tail lamps, markers checked",
      "Weatherstripping and seals noted",
    ],
  },
  {
    key: "interior",
    title: "4. Interior, Upholstery & Cabin Finish",
    icon: ClipboardList,
    fields: [
      { label: "Interior Score", type: "score" },
      { label: "Seat / Upholstery Quality", type: "select", options: ["Show", "Excellent", "Good", "Fair", "Poor"] },
      { label: "Dashboard / Instruments", type: "select", options: ["Excellent", "Good", "Fair", "Poor"] },
      { label: "Custom Audio / Electronics", type: "select", options: ["Integrated Cleanly", "Present", "Needs Work", "None"] },
    ],
    checklist: [
      "Seat upholstery and stitching reviewed",
      "Carpet, headliner, door panels checked",
      "Custom console / gauges documented",
      "Switches, controls, HVAC, infotainment noted",
    ],
  },
  {
    key: "engine",
    title: "5. Engine Compartment & Induction",
    icon: Wrench,
    fields: [
      { label: "Engine Bay Score", type: "score" },
      { label: "Engine Family", type: "text", placeholder: "LS3 / 350 SBC / Coyote / Hemi" },
      { label: "Induction", type: "select", options: ["Naturally Aspirated", "Supercharged", "Turbocharged", "Nitrous", "Other"] },
      { label: "Engine Bay Presentation", type: "select", options: ["Show", "Excellent", "Good", "Fair"] },
    ],
    checklist: [
      "Engine identification captured",
      "Wiring, plumbing, and hose routing reviewed",
      "Leaks, seepage, smoke, noises noted",
      "Cooling, accessory drive, intake, headers documented",
    ],
  },
  {
    key: "drivetrain",
    title: "6. Transmission, Driveline & Rear End",
    icon: Gauge,
    fields: [
      { label: "Drivetrain Score", type: "score" },
      { label: "Transmission Type", type: "text", placeholder: "4L80E / Tremec T56 / TH400" },
      { label: "Rear Axle / Differential", type: "text", placeholder: "12-bolt / Ford 9-inch / Dana 60" },
      { label: "Operational Condition", type: "select", options: ["Excellent", "Good", "Fair", "Poor", "Unknown"] },
    ],
    checklist: [
      "Shift quality and drivability noted",
      "Clutch / converter operation noted",
      "Driveshaft and differential reviewed",
      "Leaks, noises, vibration noted",
    ],
  },
  {
    key: "chassis",
    title: "7. Suspension, Steering, Brakes & Chassis",
    icon: ShieldCheck,
    fields: [
      { label: "Chassis Score", type: "score" },
      { label: "Front Suspension", type: "text", placeholder: "IFS / stock / tubular / coilover" },
      { label: "Rear Suspension", type: "text", placeholder: "4-link / leaf / air ride" },
      { label: "Brake System", type: "text", placeholder: "Wilwood discs / power brakes / manual discs" },
    ],
    checklist: [
      "Frame / chassis condition reviewed",
      "Suspension upgrades documented",
      "Brake hardware and condition noted",
      "Steering feel / free play / alignment concerns noted",
    ],
  },
  {
    key: "undercarriage",
    title: "8. Undercarriage, Floors & Structural Integrity",
    icon: AlertTriangle,
    fields: [
      { label: "Undercarriage Score", type: "score" },
      { label: "Floor / Structure Condition", type: "select", options: ["Excellent", "Good", "Fair", "Poor"] },
      { label: "Rust / Repairs Underneath", type: "select", options: ["None Observed", "Minor", "Moderate", "Severe"] },
    ],
    checklist: [
      "Frame rails and crossmembers checked",
      "Floor pans, trunk, bed, or cargo area reviewed",
      "Weld quality / fabrication quality documented",
      "Corrosion, impact damage, patches, and repairs noted",
    ],
  },
  {
    key: "wheels",
    title: "9. Wheels, Tires & Stance",
    icon: Car,
    fields: [
      { label: "Wheels / Tires Score", type: "score" },
      { label: "Wheel Type", type: "text", placeholder: "Billet Specialties / Rally / Steelies / Forged" },
      { label: "Tire Condition", type: "select", options: ["New", "Excellent", "Good", "Fair", "Aged"] },
      { label: "Stance / Fitment", type: "select", options: ["Excellent", "Good", "Fair", "Poor"] },
    ],
    checklist: [
      "Wheel finish and condition checked",
      "Tire age and wear reviewed",
      "Fitment, rubbing, offset, and stance noted",
      "Spare / jack / accessories noted if present",
    ],
  },
  {
    key: "function",
    title: "10. Operational Function & Road Test",
    icon: CheckCircle2,
    fields: [
      { label: "Operational Score", type: "score" },
      { label: "Starts / Idles / Drives", type: "select", options: ["Yes", "Partially", "No", "Not Tested"] },
      { label: "Road Test Performed", type: "select", options: ["Yes", "No", "Limited"] },
      { label: "Road Test Notes", type: "textarea", placeholder: "Acceleration, braking, tracking, temperatures, noises, comfort" },
    ],
    checklist: [
      "Cold start behavior noted",
      "Idle quality and temperatures reviewed",
      "Acceleration, braking, tracking, vibration noted",
      "Gauges and drivability documented",
    ],
  },
  {
    key: "documentation",
    title: "11. Documentation, Provenance & Build Records",
    icon: FileText,
    fields: [
      { label: "Documentation Score", type: "score" },
      { label: "Title Status", type: "select", options: ["Clean", "Rebuilt", "Salvage", "Missing", "Unknown"] },
      { label: "Build Receipts", type: "select", options: ["Extensive", "Partial", "Minimal", "None"] },
      { label: "Awards / Features / Media", type: "textarea", placeholder: "Show awards, magazine features, dyno sheets, provenance notes" },
    ],
    checklist: [
      "Title / registration reviewed",
      "Build receipts / invoices reviewed",
      "Prior appraisals / dyno / spec sheets noted",
      "Awards, provenance, and ownership history captured",
    ],
  },
  {
    key: "summary",
    title: "12. Summary, Red Flags & Appraiser Conclusion",
    icon: AlertTriangle,
    fields: [
      { label: "Overall Condition Score", type: "score" },
      { label: "Authenticity / Cohesion", type: "select", options: ["Excellent", "Good", "Fair", "Poor"] },
      { label: "Red Flags", type: "textarea", placeholder: "Frame damage, title issues, rust, drivability, workmanship concerns" },
      { label: "Appraiser Summary", type: "textarea", placeholder: "Overall custom build quality, use-case, market impression, and recommendation" },
    ],
    checklist: [
      "Material defects summarized",
      "Positive build highlights summarized",
      "Red flags clearly disclosed",
      "Overall conclusion documented",
    ],
  },
];

const scoreColor = (score) => {
  if (score >= 9) return "text-emerald-700";
  if (score >= 7) return "text-blue-700";
  if (score >= 4) return "text-amber-700";
  return "text-red-700";
};

function ScoreButtons({ value, onChange }) {
  return (
    <div className="flex flex-wrap gap-2">
      {[0,1,2,3,4,5,6,7,8,9,10].map((n) => (
        <button
          key={n}
          type="button"
          onClick={() => onChange(n)}
          className={`h-9 w-9 rounded-full border text-sm font-semibold transition ${value === n ? "border-slate-900 bg-slate-900 text-white" : "border-slate-300 bg-white text-slate-700 hover:border-slate-500"}`}
        >
          {n}
        </button>
      ))}
    </div>
  );
}

export default function NexusCustomVehicleInspectionForm() {
  const [activeTab, setActiveTab] = useState(systems[0].key);
  const [scores, setScores] = useState({
    body: 8,
    interior: 8,
    engine: 8,
    drivetrain: 8,
    chassis: 8,
    undercarriage: 7,
    wheels: 8,
    function: 8,
    documentation: 7,
    glass: 8,
    identity: 9,
    summary: 8,
  });

  const overall = useMemo(() => {
    const vals = Object.values(scores);
    return (vals.reduce((a, b) => a + b, 0) / vals.length).toFixed(1);
  }, [scores]);

  const completed = useMemo(() => {
    return Object.values(scores).filter((v) => typeof v === "number").length;
  }, [scores]);

  const progress = Math.round((completed / systems.length) * 100);

  return (
    <div className="min-h-screen bg-slate-100 text-slate-900">
      <div className="border-b bg-gradient-to-r from-slate-950 via-slate-900 to-blue-950 text-white">
        <div className="mx-auto flex max-w-7xl items-center justify-between px-4 py-5 md:px-6">
          <div>
            <div className="text-sm tracking-[0.25em] text-slate-300">ONE CLASSICS</div>
            <div className="text-4xl font-bold leading-none">NEXUS</div>
          </div>
          <div className="hidden items-center gap-6 md:flex">
            <span className="text-sm text-slate-300">Field Inspection Form</span>
            <Badge className="bg-white/10 text-white hover:bg-white/10">Custom Vehicle</Badge>
          </div>
        </div>
      </div>

      <div className="mx-auto grid max-w-7xl gap-6 px-4 py-6 md:grid-cols-[280px_minmax(0,1fr)_320px] md:px-6">
        <motion.div initial={{ opacity: 0, x: -10 }} animate={{ opacity: 1, x: 0 }}>
          <Card className="rounded-2xl border-slate-200 shadow-sm">
            <CardHeader>
              <CardTitle className="text-xl">Inspection Sections</CardTitle>
              <CardDescription>Nexus 12-System architecture</CardDescription>
            </CardHeader>
            <CardContent className="space-y-3">
              <div className="rounded-xl bg-slate-50 p-3">
                <div className="mb-2 flex items-center justify-between text-sm font-medium">
                  <span>Completion</span>
                  <span>{progress}%</span>
                </div>
                <Progress value={progress} className="h-2" />
              </div>
              <div className="space-y-2">
                {systems.map((system, idx) => {
                  const Icon = system.icon;
                  const isActive = activeTab === system.key;
                  return (
                    <button
                      key={system.key}
                      onClick={() => setActiveTab(system.key)}
                      className={`flex w-full items-start gap-3 rounded-xl border px-3 py-3 text-left transition ${isActive ? "border-slate-900 bg-slate-900 text-white" : "border-slate-200 bg-white hover:border-slate-400"}`}
                    >
                      <Icon className={`mt-0.5 h-4 w-4 shrink-0 ${isActive ? "text-white" : "text-slate-600"}`} />
                      <div className="min-w-0">
                        <div className="text-sm font-semibold leading-snug">{idx + 1}. {system.title.replace(/^\d+\.\s/, "")}</div>
                      </div>
                    </button>
                  );
                })}
              </div>
            </CardContent>
          </Card>
        </motion.div>

        <motion.div initial={{ opacity: 0, y: 10 }} animate={{ opacity: 1, y: 0 }} className="space-y-6">
          <Card className="rounded-2xl border-slate-200 shadow-sm">
            <CardHeader className="flex flex-col gap-4 lg:flex-row lg:items-start lg:justify-between">
              <div>
                <CardTitle className="text-2xl">Custom Vehicle Inspection Input</CardTitle>
                <CardDescription>Request #1139 · 1970 Chevrolet K10 · Owner / Client field intake</CardDescription>
              </div>
              <div className="grid gap-2 text-sm text-slate-600">
                <div><span className="font-semibold text-slate-900">Inspector:</span> ONE Classics Field Appraiser</div>
                <div><span className="font-semibold text-slate-900">Location:</span> Meridian, Idaho</div>
                <div><span className="font-semibold text-slate-900">Mode:</span> Mobile / tablet-ready form</div>
              </div>
            </CardHeader>
          </Card>

          <Tabs value={activeTab} onValueChange={setActiveTab} className="w-full">
            <TabsList className="hidden" />
            {systems.map((system) => (
              <TabsContent key={system.key} value={system.key} className="mt-0">
                <Card className="rounded-2xl border-slate-200 shadow-sm">
                  <CardHeader>
                    <CardTitle className="text-2xl">{system.title}</CardTitle>
                    <CardDescription>Capture field observations, scores, photos, and appraiser notes.</CardDescription>
                  </CardHeader>
                  <CardContent className="space-y-6">
                    <div className="grid gap-4 md:grid-cols-2">
                      {system.fields.map((field) => {
                        const id = `${system.key}-${field.label}`;
                        const span = field.type === "textarea" ? "md:col-span-2" : "";
                        if (field.type === "score") {
                          return (
                            <div key={id} className="md:col-span-2 rounded-2xl border border-slate-200 bg-slate-50 p-4">
                              <div className="mb-3 flex items-center justify-between gap-4">
                                <Label className="text-base font-semibold">{field.label}</Label>
                                <div className={`text-lg font-bold ${scoreColor(scores[system.key] ?? 0)}`}>{scores[system.key] ?? 0}/10</div>
                              </div>
                              <ScoreButtons value={scores[system.key] ?? 0} onChange={(n) => setScores((prev) => ({ ...prev, [system.key]: n }))} />
                            </div>
                          );
                        }
                        if (field.type === "select") {
                          return (
                            <div key={id} className={span}>
                              <Label className="mb-2 block">{field.label}</Label>
                              <Select>
                                <SelectTrigger className="h-11 rounded-xl bg-white">
                                  <SelectValue placeholder={`Select ${field.label.toLowerCase()}`} />
                                </SelectTrigger>
                                <SelectContent>
                                  {field.options?.map((opt) => <SelectItem key={opt} value={opt}>{opt}</SelectItem>)}
                                </SelectContent>
                              </Select>
                            </div>
                          );
                        }
                        if (field.type === "textarea") {
                          return (
                            <div key={id} className={span}>
                              <Label className="mb-2 block">{field.label}</Label>
                              <Textarea className="min-h-[120px] rounded-2xl bg-white" placeholder={field.placeholder} />
                            </div>
                          );
                        }
                        return (
                          <div key={id} className={span}>
                            <Label className="mb-2 block">{field.label}</Label>
                            <Input className="h-11 rounded-xl bg-white" placeholder={field.placeholder} />
                          </div>
                        );
                      })}
                    </div>

                    <div className="grid gap-6 lg:grid-cols-[1.1fr_0.9fr]">
                      <Card className="rounded-2xl border-slate-200 bg-slate-50 shadow-none">
                        <CardHeader>
                          <CardTitle className="text-lg">Inspection Checklist</CardTitle>
                          <CardDescription>Mark completed observations for this system.</CardDescription>
                        </CardHeader>
                        <CardContent className="space-y-4">
                          {system.checklist.map((item, i) => (
                            <div key={i} className="flex items-start gap-3 rounded-xl bg-white p-3">
                              <Checkbox id={`${system.key}-check-${i}`} className="mt-1" />
                              <Label htmlFor={`${system.key}-check-${i}`} className="text-sm leading-6">{item}</Label>
                            </div>
                          ))}
                          <div>
                            <Label className="mb-2 block">Section Notes</Label>
                            <Textarea className="min-h-[120px] rounded-2xl bg-white" placeholder="Add detailed field notes, workmanship observations, defects, strengths, measurements, or appraiser comments." />
                          </div>
                        </CardContent>
                      </Card>

                      <Card className="rounded-2xl border-slate-200 bg-slate-50 shadow-none">
                        <CardHeader>
                          <CardTitle className="text-lg">Photo & Evidence Capture</CardTitle>
                          <CardDescription>Attach images and supporting documentation while in the field.</CardDescription>
                        </CardHeader>
                        <CardContent className="space-y-4">
                          <div className="rounded-2xl border-2 border-dashed border-slate-300 bg-white p-6 text-center">
                            <Upload className="mx-auto mb-3 h-8 w-8 text-slate-500" />
                            <div className="font-medium">Drop photos here or tap to upload</div>
                            <div className="mt-1 text-sm text-slate-500">Supports VIN, exterior, undercarriage, engine bay, interior, receipts, and title images.</div>
                            <Button className="mt-4 rounded-xl">Add Photos</Button>
                          </div>
                          <div className="grid grid-cols-3 gap-3">
                            {['Front 3/4','Rear 3/4','Engine Bay','Interior','Undercarriage','VIN'].map((label) => (
                              <div key={label} className="rounded-2xl border border-slate-200 bg-white p-3 text-center text-xs font-medium text-slate-600">
                                <div className="mb-2 aspect-[4/3] rounded-xl bg-slate-100" />
                                {label}
                              </div>
                            ))}
                          </div>
                          <div>
                            <Label className="mb-2 block">Evidence Reference / File Notes</Label>
                            <Textarea className="min-h-[100px] rounded-2xl bg-white" placeholder="Document where evidence came from, filename conventions, missing-photo notes, or follow-up document requests." />
                          </div>
                        </CardContent>
                      </Card>
                    </div>
                  </CardContent>
                </Card>
              </TabsContent>
            ))}
          </Tabs>
        </motion.div>

        <motion.div initial={{ opacity: 0, x: 10 }} animate={{ opacity: 1, x: 0 }} className="space-y-6">
          <Card className="rounded-2xl border-slate-200 shadow-sm">
            <CardHeader>
              <CardTitle>Inspection Snapshot</CardTitle>
              <CardDescription>Live scoring and field readiness summary</CardDescription>
            </CardHeader>
            <CardContent className="space-y-4">
              <div className="rounded-2xl bg-slate-50 p-4">
                <div className="text-sm text-slate-500">Overall Condition Index</div>
                <div className="mt-1 text-3xl font-bold text-slate-900">{overall}</div>
                <div className="mt-1 text-sm text-slate-600">0–10 field score average across the 12 systems</div>
              </div>
              <div className="space-y-3">
                {systems.map((s) => (
                  <div key={s.key} className="flex items-center justify-between gap-3 rounded-xl border border-slate-200 px-3 py-2">
                    <div className="min-w-0 text-sm font-medium text-slate-700">{s.title.replace(/^\d+\.\s/, "")}</div>
                    <Badge variant="secondary" className={scoreColor(scores[s.key] ?? 0)}>{scores[s.key] ?? 0}/10</Badge>
                  </div>
                ))}
              </div>
            </CardContent>
          </Card>

          <Card className="rounded-2xl border-slate-200 shadow-sm">
            <CardHeader>
              <CardTitle>Red Flag Controls</CardTitle>
              <CardDescription>Capture material issues affecting value or review priority</CardDescription>
            </CardHeader>
            <CardContent className="space-y-3">
              {[
                "Title problem / mismatch",
                "Frame or structural damage",
                "Severe corrosion",
                "Poor workmanship / unsafe build",
                "Mechanical failure / not operational",
                "Missing documentation",
              ].map((item) => (
                <div key={item} className="flex items-start gap-3 rounded-xl border border-slate-200 p-3">
                  <Checkbox id={item} />
                  <Label htmlFor={item} className="text-sm leading-6">{item}</Label>
                </div>
              ))}
              <Textarea className="min-h-[100px] rounded-2xl" placeholder="Add red-flag narrative, urgency, or recommended follow-up." />
            </CardContent>
          </Card>

          <Card className="rounded-2xl border-slate-200 shadow-sm">
            <CardHeader>
              <CardTitle>Actions</CardTitle>
              <CardDescription>Designed for field use on tablet or laptop</CardDescription>
            </CardHeader>
            <CardContent className="space-y-3">
              <Button className="h-11 w-full justify-start rounded-xl"><Save className="mr-2 h-4 w-4" /> Save Draft</Button>
              <Button variant="secondary" className="h-11 w-full justify-start rounded-xl"><ClipboardList className="mr-2 h-4 w-4" /> Mark Section Reviewed</Button>
              <Button variant="outline" className="h-11 w-full justify-start rounded-xl"><Camera className="mr-2 h-4 w-4" /> Request Missing Photos</Button>
              <Button variant="outline" className="h-11 w-full justify-start rounded-xl"><AlertTriangle className="mr-2 h-4 w-4" /> Send Back for Clarification</Button>
              <Button className="h-11 w-full justify-start rounded-xl bg-emerald-600 hover:bg-emerald-700"><CheckCircle2 className="mr-2 h-4 w-4" /> Finalize Inspection Input</Button>
            </CardContent>
          </Card>
        </motion.div>
      </div>
    </div>
  );
}
