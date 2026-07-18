id-card-access-level-virology = Virology

job-description-virologist = Research, contain, and treat infectious diseases. Maintain sample custody and outbreak protocol.

sol-pathogen-flu-name = Station Flu
sol-pathogen-flu-desc = A contagious respiratory illness causing fever, coughing, and sneezing.
sol-pathogen-sepsis-name = Wound Sepsis
sol-pathogen-sepsis-desc = A surgical and contact infection that can progress to organ stress.
sol-pathogen-bioagent-name = Engineered Bioagent
sol-pathogen-bioagent-desc = A virulent engineered pathogen used in bioterror scenarios.

sol-pathogen-unknown-description = An unidentified pathogen.
sol-pathogen-exposed = You feel a chill. Something may have gotten into your system...
sol-pathogen-symptoms-start = You start feeling the effects of {$disease}.
sol-pathogen-recovered = You seem to have recovered from {$disease}.

sol-swab-already-used = This swab has already been used.
sol-swab-collected = You collect a sample from {$target}.
sol-diagnoser-report-negative = RESULT: No pathogen detected.
sol-diagnoser-report-positive = RESULT: {$disease} detected.
    Stage: {$stage}
    Dose: {$dose}
    Blood sample: {$blood}
sol-diagnoser-report-inconclusive = RESULT: Inconclusive.
sol-diagnoser-report-name = Disease Diagnosis Report
sol-diagnoser-complete = Diagnosis complete. Report printed.
sol-vaccinator-need-sample = Insert a valid pathogen sample.
sol-vaccinator-bad-sample = Sample quality insufficient for vaccine production.
sol-vaccinator-produced = Vaccine synthesized.
sol-vaccine-failed = Vaccination failed. Patient may already be symptomatic.
sol-vaccine-applied = You vaccinate {$target}.
sol-sample-swab = Contains a mucosal/surface sample.
sol-sample-blood = Contains an unprocessed blood sample.
sol-sample-blood-centrifuged = Contains a centrifuged blood panel.

sol-blood-vial-full = This vial already contains a sample.
sol-blood-drawn = You draw blood from {$target}.
sol-blood-panel-not-ready = Blood panel not centrifuged.
sol-blood-panel-pathogen-negative = Blood panel: no pathogen markers detected.
sol-blood-panel-inconclusive = Blood panel: inconclusive.
sol-blood-panel-full = Blood panel:
    Pathogen: {$disease}
    Stage: {$stage}
    Dose: {$dose}
    Antibodies: {$immunity}
    Organ function: {$organs}
sol-blood-panel-organs-unknown = Unknown (source unavailable)
sol-blood-panel-organs-none = No organs detected
sol-blood-panel-organs-summary = damaged={$damaged}, failing={$failing}, missing={$missing}
sol-ppe-safe-doff = You carefully remove the contaminated PPE while gloved.
sol-ppe-unsafe-doff = Contaminated PPE contacts your bare skin as you remove it!

sol-surgery-tool-sterile = [color=green]Sterile[/color]
sol-surgery-tool-disinfected = [color=yellow]Disinfected[/color]
sol-surgery-tool-dirty = [color=red]Dirty[/color]
sol-surgery-tool-contaminated = [color=orange]Pathogen contamination detected.[/color]
sol-surgery-tool-wash-verb = Wash tool
sol-surgery-tool-washed = You wash the tool. It is cleaner, but not fully sterile.
sol-surgery-tool-sterilized = The tool is now sterile.
sol-surgery-tool-disinfected-popup = The tool has been disinfected.
sol-surgery-dirty-tool-warning = Your surgical tools are dirty! Sterilize them before the next patient.
sol-surgery-failed-infection-risk = The failed step may have contaminated the wound.
sol-surgery-inspect-asepsis-verb = Inspect asepsis
sol-surgery-asepsis-status = Held non-sterile tools: {$dirty}. Surgical mask: { $masked ->
    [true] worn
   *[false] not worn
}
sol-surgery-window-title = Surgery
sol-surgery-window-title-part = Surgery - {$part}
sol-surgery-no-actions = No available surgical actions for this part.
sol-surgery-action-prerequisite = Prerequisite: {$procedure} (for {$goal})
sol-surgery-action-dirty-tools = Dirty tools — infection risk elevated
sol-surgery-dirty-confirm = Your tools are not sterile. Proceed with [bold]{$surgery}[/bold] — {$step}?
sol-surgery-asepsis-banner = Tools: [color=green]{$sterile} sterile[/color], [color=orange]{$dirty} dirty[/color] · Mask: { $masked ->
    [true] [color=green]on[/color]
    *[false] [color=red]off[/color]
}
sol-surgery-must-lie-down = [color=red][font size=16]They need to be lying down![/font][/color]
sol-surgery-needs-table = Needs operating table
sol-surgery-remove-armor = Remove their armor!
sol-surgery-missing-tool = Missing tool
sol-surgery-disabled-tool = Disabled tool
sol-surgery-item-too-high = Item too high
sol-surgery-missing-reagent = Missing reagent
sol-surgery-missing-limb = Can't attach as limb
sol-surgery-cannot-perform = Cannot perform this step
sol-surgery-dirty-tool-before-step = Operating with non-sterile tools increases infection risk.

sol-sterilizer-unpowered = The sterilization airlock is unpowered.
sol-sterilizer-doors-open = Close the airlock to begin sterilization.
sol-sterilizer-started = Sterilization cycle started.
sol-sterilizer-interrupted = Sterilization cycle interrupted. Contaminants remain.
sol-sterilizer-complete = Sterilization cycle complete.

sol-health-analyzer-organs-header = Organs
sol-health-analyzer-organ-line = {$organ}: {$status}
sol-health-analyzer-damage-tab = Damage
sol-health-analyzer-organs-tab = Organs
sol-health-analyzer-no-organs = No organs detected.
sol-health-analyzer-debug-header = DEBUG VIROLOGY

sol-allergy-unknown-description = An unidentified allergy.
sol-allergy-peanut-name = Peanut Allergy
sol-allergy-peanut-desc = Severe reaction to peanut products.
sol-allergy-latex-name = Latex Allergy
sol-allergy-latex-desc = Mild reaction to latex exposure.
sol-allergy-reaction = You are having an allergic reaction to {$allergy}!
sol-allergy-anaphylaxis = ANAPHYLAXIS! Seek emergency treatment!

sol-bioterror-briefing = You are a bioterrorist. Establish a clandestine lab, culture environmental microbes into a custom strain, and deploy physical payloads. Avoid early detection.
sol-bioterror-briefing-head = You are the Head Bioterrorist. You carry the portable lab flatpacks. Lead the cell, choose a hideout, and coordinate synthesis and deployment.
sol-bioterror-briefing-member = You are a Bioterrorist cell member. Help establish the lab, scrape environmental microbes, culture traits, and deploy manufactured payloads.
sol-bioterror-contaminate-food = Contaminate with pathogen
sol-bioterror-release-airborne = Release airborne pathogen
sol-bioterror-deploy-aerosol = Release aerosol culture
sol-bioterror-food-contaminated = You contaminate the target.
sol-bioterror-surface-contaminated = You apply culture to the surface.
sol-bioterror-airborne-released = You release an airborne pathogen load.
sol-bioterror-no-station = Virology systems are not active here.
sol-bioterror-payload-invalid = This culture is inert or unrecognized.
sol-bioterror-scrape-invalid = There is nothing useful to scrape here.
sol-bioterror-scrape-depleted = This source has been scraped clean for now.
sol-bioterror-scrape-success = You collect an environmental microbial sample.
sol-bioterror-sample-name = microbial sample ({$source})
sol-bioterror-lab-unpowered = The machine has no power.
sol-bioterror-analyzer-result = Analysis: chassis={$chassis}, quality={$quality}, contaminated={$contaminated}, traits={$traits}
sol-bioterror-analyzer-no-traits = none detected
sol-bioterror-analyzer-examine = Accepts environmental scrapings for trait/chassis analysis.
sol-bioterror-incubator-need-analyzed = Insert an analyzed microbial sample.
sol-bioterror-incubator-busy = The incubator is busy.
sol-bioterror-incubator-need-nutrient = Add culture nutrient to the machine tank.
sol-bioterror-incubator-started = Culture cycle started.
sol-bioterror-incubator-complete = Culture ready.
sol-bioterror-incubator-spoiled = Power loss spoiled the culture.
sol-bioterror-incubator-overgrown = An unattended culture overgrew and contaminated the area!
sol-bioterror-incubator-examine = Needs analyzed samples and nutrient medium.
sol-bioterror-incubator-examine-running = Culture cycle in progress.
sol-bioterror-synth-need-culture = Insert a culture vial, then begin synthesis.
sol-bioterror-synth-need-chassis = Load a chassis culture first.
sol-bioterror-synth-need-stabilizer = Add culture stabilizer to the machine tank.
sol-bioterror-synth-chassis-loaded = Chassis loaded: {$chassis}
sol-bioterror-synth-traits-loaded = Trait isolates loaded into the recipe.
sol-bioterror-synth-invalid = Synthesis rejected: {$error}
sol-bioterror-synth-started = Synthesis cycle started.
sol-bioterror-synth-complete = Strain {$strain} synthesized. Ampoules dispensed.
sol-bioterror-synth-failed = Synthesis failed catastrophically!
sol-bioterror-synth-spoiled = Power loss ruined the synthesis batch.
sol-bioterror-synth-busy = The synthesizer is busy.
sol-bioterror-synth-begin-verb = Begin synthesis
sol-bioterror-synth-examine = Pending chassis: {$chassis}. Traits: {$traits}.
sol-bioterror-ampoule-name = culture ampoule ({$strain})
sol-bioterror-round-end-agent-name = bioterrorist
sol-bioterror-roundend-header = Bioterror cell results:
sol-bioterror-roundend-lab = Lab established off-shuttle: {$established} (analyzer={$analyzer}, incubator={$incubator}, synthesizer={$synthesizer})
sol-bioterror-roundend-strain = Synthesized strain: {$strain}
sol-bioterror-roundend-none = none
sol-bioterror-roundend-deployed = Deployed load: {$load} / {$required}
sol-bioterror-roundend-medical = Diagnosed: {$diagnosed}. Vaccine created: {$vaccine}.
sol-bioterror-roundend-survivors = Living cell members: {$count}
sol-bioterror-objective-lab-title = Establish the clandestine laboratory
sol-bioterror-objective-lab-desc = Deploy and operate all three lab machines away from the infiltration shuttle.
sol-bioterror-objective-synth-title = Synthesize a viable custom strain
sol-bioterror-objective-synth-desc = Culture environmental traits and assemble a bounded custom pathogen.
sol-bioterror-objective-deploy-title = Deploy manufactured cultures
sol-bioterror-objective-deploy-desc = Manufacture physical payloads and release a minimum infectious load.
sol-bioterror-objective-delay-title = Delay confirmed diagnosis
sol-bioterror-objective-delay-desc = Prevent a matching vaccine for as long as possible after first deployment.
sol-bioterror-objective-survive-title = Keep the cell alive
sol-bioterror-objective-survive-desc = At least one cell member must survive.

sol-pathogen-trait-unknown-description = An unidentified microbial trait.
sol-pathogen-trait-contact-name = Contact persistence
sol-pathogen-trait-contact-desc = Improves surface contact transmission.
sol-pathogen-trait-airborne-name = Airborne adaptation
sol-pathogen-trait-airborne-desc = Enables aerosol transmission.
sol-pathogen-trait-ingestion-name = Enteric adaptation
sol-pathogen-trait-ingestion-desc = Enables foodborne infection.
sol-pathogen-trait-aerosol-name = Aerosol stability
sol-pathogen-trait-aerosol-desc = Improves environmental persistence in air.
sol-pathogen-trait-cough-name = Cough shedding
sol-pathogen-trait-cough-desc = Increases cough-driven shedding.
sol-pathogen-trait-yield-name = Culture yield
sol-pathogen-trait-yield-desc = Speeds incubation of isolates.
sol-pathogen-trait-slow-name = Slow incubation
sol-pathogen-trait-slow-desc = Lengthens incubation and reduces detectability.
sol-pathogen-trait-growth-name = Environmental growth
sol-pathogen-trait-growth-desc = Survives longer on surfaces and waste.
sol-pathogen-trait-virulent-name = Heightened virulence
sol-pathogen-trait-virulent-desc = Increases lethality and infectivity.
sol-pathogen-trait-liver-name = Hepatic tropism
sol-pathogen-trait-liver-desc = Targets liver tissue.
sol-pathogen-trait-persistent-name = Environmental persistence
sol-pathogen-trait-persistent-desc = Resists environmental decay.
sol-pathogen-trait-sterilant-name = Sterilant resistance
sol-pathogen-trait-sterilant-desc = Reduces sterilant effectiveness.

chat-radio-bioterror = Bioterror

sol-gamemode-virology-title = Virology
sol-gamemode-virology-desc = A bioterror cell infiltrates with a portable lab, synthesizes custom strains from environmental scrapings, and deploys physical payloads. Medical must diagnose, contain, and vaccinate. Major syndicate antags are disabled; thief and selected creature threats may still appear.
sol-virology-preset-no-ready-virologist = Virology cannot start without an eligible ready player who has Virologist set to High priority on a virology-enabled station.

reagent-name-sol-antiviral = antiviral
reagent-desc-sol-antiviral = Broad-spectrum antiviral treatment for Sol pathogens.
reagent-name-sol-antipyretic = antipyretic
reagent-desc-sol-antipyretic = Reduces fever.
reagent-name-sol-sterilizine = sterilizine
reagent-desc-sol-sterilizine = Strong sterilant for tools, surfaces, and PPE.
reagent-name-sol-antihistamine = antihistamine
reagent-desc-sol-antihistamine = Reduces mild allergic reactions.
reagent-name-sol-epinephrine-allergy = allergy epinephrine
reagent-desc-sol-epinephrine-allergy = Emergency treatment for severe allergic reactions.
reagent-name-sol-culture-nutrient = culture nutrient
reagent-desc-sol-culture-nutrient = Nutrient medium used to culture environmental microbial isolates.
reagent-name-sol-culture-stabilizer = culture stabilizer
reagent-desc-sol-culture-stabilizer = Stabilizes custom pathogen synthesis batches.

guide-entry-sl-medical-sop-virologist = Virologist
guide-entry-sl-medical-sop-virology-outbreak = Virology and Outbreak Procedure
guide-entry-bioterrorists = Bioterrorists

roles-antag-bioterrorist-name = Bioterrorist
roles-antag-bioterrorist-objective = Help the cell culture environmental microbes into deployable custom strains.
roles-antag-head-bioterrorist-name = Head Bioterrorist
roles-antag-head-bioterrorist-objective = Lead the cell, deploy the portable lab, and synthesize a custom pathogen.
