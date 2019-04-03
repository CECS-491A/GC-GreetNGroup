<template>
    <div id="create-event">
        <v-app id="event-creation-form">
            <v-layout justify-center>
                <v-flex xs12 sm10 md8 lg6>
                    <v-card ref="form">
                        <v-card-text>
                            <v-text-field
                            ref="name"
                            v-model="name"
                            :rules="[() => !!name || 'This field is required',
                                    () => !!name && name.length <= 50 || 'Title must be less than 50 characters', titleCheck
                                    ]"
                            :error-messages="errorMessages"
                            label="Event Title"
                            placeholder="Pizza Party"
                            counter="50"
                            required
                            ></v-text-field>
                            <p>{{ radios || 'null' }}</p>
                            <v-radio-group v-model="radios" :mandatory="true">
                                <v-radio label="Online Event" value="isOnline"></v-radio>
                                <v-radio label="Physical Event" value="isPhysical"></v-radio>
                            </v-radio-group>
                            <v-text-field
                            ref="address"
                            v-model="address"
                            :rules="[
                                () => !!address || 'This field is required'
                            ]"
                            label="Address Line"
                            placeholder="Snowy Rock Pl"
                            required
                            ></v-text-field>
                            <v-text-field
                            ref="city"
                            v-model="city"
                            :rules="[() => !!city || 'This field is required', addressCheck]"
                            label="City"
                            placeholder="El Paso"
                            required
                            ></v-text-field>
                            <v-text-field
                            ref="state"
                            v-model="state"
                            :rules="[() => !!state || 'This field is required']"
                            label="State"
                            required
                            placeholder="Texas"
                            ></v-text-field>
                            <v-text-field
                            ref="zip"
                            v-model="zip"
                            :rules="[() => !!zip || 'This field is required']"
                            label="ZIP / Postal Code"
                            required
                            placeholder="79938"
                            ></v-text-field>
                            <v-menu
                                ref="menu"
                                v-model="menu"
                                :close-on-content-click="false"
                                :nudge-right="40"
                                :return-value.sync="date"
                                lazy
                                transition="scale-transition"
                                offset-y
                                full-width
                                min-width="290px"
                            >
                                <template v-slot:activator="{ on }">
                                <v-text-field v-model="date"
                                    ref="date"
                                    label="Date of Event"
                                    prepend-icon="event"
                                    readonly
                                    v-validate="'required'"
                                    v-on="on"
                                ></v-text-field>
                                </template>
                            <v-date-picker id="datepicker"
                                v-model="date"
                                no-title scrollable
                                :allowed-dates="allowedDates"
                                class="mt-3"
                                min=""
                            >
                                <v-spacer></v-spacer>
                                <v-btn flat color="primary" @click="menu = false">Cancel</v-btn>
                                <v-btn flat color="primary" @click="$refs.menu.save(date)">OK</v-btn>
                                </v-date-picker>
                                </v-menu>
                                <v-layout row wrap>
                            <v-flex md12 lg6>
                                <v-time-picker id="timepicker"
                                v-model="time"
                                :allowed-hours="startAllowedHours"
                                :allowed-minutes="startAllowedStep"
                                scrollable
                                class="mt-3"             
                                min=""
                                ></v-time-picker>
                            </v-flex>
                            </v-layout>
                        </v-card-text>
                        <v-divider class="mt-5"></v-divider>
                        <v-card-actions>
                            <v-btn flat>Cancel</v-btn>
                            <v-spacer></v-spacer>
                            <v-slide-x-reverse-transition>
                            <v-tooltip
                                v-if="formHasErrors"
                                left
                            >
                                <template v-slot:activator="{ on }">
                                <v-btn
                                    icon
                                    class="my-0"
                                    @click="resetForm"
                                    v-on="on"
                                >
                                    <v-icon>refresh</v-icon>
                                </v-btn>
                                </template>
                                <span>Refresh form</span>
                            </v-tooltip>
                            </v-slide-x-reverse-transition>
                            <v-btn color="primary" flat @click="submit">Submit</v-btn>
                        </v-card-actions>
                    </v-card>
                </v-flex>
            </v-layout>
        </v-app>
    </div>
</template>

<script>
    var todaysDate = new Date();

    var dd = todaysDate.getDate();
    var mm = todaysDate.getMonth() + 1;
    var yyyy = todaysDate.getFullYear();
    var hh = todaysDate.getHours();
    var mm = todaysDate.getMinutes();

    if (dd < 10) {
    dd = "0" + dd;
    }
    if (mm < 10) {
    mm = "0" + mm;
    }

    var stringDate = yyyy + "-" + mm + "-" + dd;
    var timeString = hh + ":" + mm;
    document.getElementById("datepicker").setAttribute("min", stringDate);
    document.getElementById("timepicker").setAttribute("min", timeString);
    
    new Vue({
        el: 'create-event',
        data: () => ({
            errorMessages: '',
            name: null,
            radios: null,
            address: null,
            city: null,
            state: null,
            zip: null,
            date: null,
            menu: false,
            startTime: null,
            startTimeStep: null,
            formHasErrors: false
        }),

        computed: {
            form () {
                return {
                    name: this.name,
                    radios: this.radios,
                    address: this.address,
                    city: this.city,
                    state: this.state,
                    zip: this.zip,
                    date: new Date().toISOString().substr(0, 10),
                    startTime: this.startTime,
                    startTimeStep: this.startTimeStep,
                    startDateTime: new Date(date.getFullYear() + "-" + date.getMonth() + "-" + 
                    date.getDate() + " " + startTime + ":" + startTimeStep)
                }
            }
        },

        watch: {
            name () {
            this.errorMessages = ''
            }
        },

        methods: {
            allowedDates: val => parseInt(val.split("-")) >= 0,
            startAllowedHours: v => v % 1 === 0,
            startAllowedStep: m => m % 10 === 0 || m % 10 === 5,
            nameCheck () {
                this.errorMessages = this.address && !this.name
                ? 'You must enter an event name'
                : ''

            return true
            },
            resetForm () {
            this.errorMessages = []
            this.formHasErrors = false

            Object.keys(this.form).forEach(f => {
                this.$refs[f].reset()
            })
            },
            submit () {
            this.formHasErrors = false

            Object.keys(this.form).forEach(f => {
                if (!this.form[f]) this.formHasErrors = true

                this.$refs[f].validate(true)
            })
            }
        }
    })
</script>