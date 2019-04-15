<template>
    <div id="create-event">
    <v-app id="inspire">
        <v-layout justify-center>
        <v-flex xs12 sm10 md8 lg6>
            <v-card ref="form">
            <v-card-text>
                <v-text-field
                    ref="name"
                    v-model="name"
                    :rules="[() => !!name || 'This field is required',
                            () => !!name && name.length < 50 || 'Title must be less than 50 characters', titleCheck
                            ]"
                    :error-messages="errorMessages"
                    label="Event Title"
                    placeholder="Pizza Party"
                    counter="50"
                    required
                ></v-text-field>
                <v-text-field
                    ref="address"
                    v-model="address"
                    :rules="[
                            () => !!address || 'This field is required',
                            addressCheck
                            ]"
                    :error-messages="errorMessages"
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
                    :rules="[() => !!state || 'This field is required', addressCheck]"
                    label="State/Province/Region"
                    required
                    placeholder="TX"
                ></v-text-field>
                <v-text-field
                    ref="zip"
                    v-model="zip"
                    :rules="[() => !!zip || 'This field is required', addressCheck]"
                    label="ZIP / Postal Code"
                    required
                    placeholder="79938"
                ></v-text-field>
                <v-textarea
                    ref="description"
                    v-model="description"
                    :rules="[() => description.length < 250 || 'Description must be less than 250 characters']"
                    :error-messages="errorMessages"
                    label="Event Description (Optional)"
                    placeholder="Bring your own beverages!"
                    counter="250"
                ></v-textarea>
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
                            :rules="[() => !!date || 'This field is required']"
                            readonly
                            required
                            v-on="on"
                        ></v-text-field>
                    </template>
                    <v-date-picker id="datepicker"
                        ref="datepicker"
                        v-model="date"
                        no-title scrollable
                        :allowed-dates="allowedDates"
                        class="mt-3"
                        :min="minDate"
                        >
                        <v-spacer></v-spacer>
                        <v-btn flat color="primary" @click="menu = false">Cancel</v-btn>
                        <v-btn flat color="primary" @click="$refs.menu.save(date)">OK</v-btn>
                    </v-date-picker>
                </v-menu>
                <v-menu
                    ref="timeMenu"
                    v-model="timeMenu"
                    :close-on-content-click="false"
                    :nudge-right="40"
                    :return-value.sync="startTime"
                    lazy
                    transition="scale-transition"
                    offset-y
                    full-width
                    max-width="290px"
                    min-width="290px"
                    >
                    <template v-slot:activator="{ on }">
                        <v-text-field v-model="startTime"
                            ref="startTime"
                            label="Time of Event"
                            prepend-icon="access_time"
                            :rules="[() => !!startTime || 'This field is required']"
                            readonly
                            required
                            v-on="on"
                        ></v-text-field>
                    </template>
                    <v-time-picker id="timepicker"
                        ref="timepicker"
                        v-model="startTime"
                        v-if="timeMenu"
                        :allowed-hours="startAllowedHours"
                        :allowed-minutes="startAllowedStep"
                        scrollable
                        class="mt-3"
                        :min="minTime"
                        full-width
                        >
                        <v-spacer></v-spacer>
                        <v-btn flat color="primary" @click="timeMenu = false">Cancel</v-btn>
                        <v-btn flat color="primary" @click="$refs.timeMenu.save(startTime)">OK</v-btn>
                        </v-time-picker>
                </v-menu>
                <v-dialog v-model="dialog" persistent scrollable max-width="300px">
                    <template v-slot:activator="{ on }">
                        <v-btn color="primary" dark v-on="on">Select Tags Associated with Your Event</v-btn>
                        <p>{{ selected }}</p>
                    </template>
                    <v-card>
                        <v-card-title>Tags</v-card-title>
                        <v-divider></v-divider>
                        <v-card-text style="height: 300px;">
                            <v-container fluid>
                            <v-checkbox v-model="selected" label="Outdoors" value="Outdoors"></v-checkbox>
                            <v-checkbox v-model="selected" label="Indoors" value="Indoors"></v-checkbox>
                            <v-checkbox v-model="selected" label="Music" value="Music"></v-checkbox>
                            <v-checkbox v-model="selected" label="Games" value="Games"></v-checkbox>
                            <v-checkbox v-model="selected" label="Fitness" value="Fitness"></v-checkbox>
                            <v-checkbox v-model="selected" label="Art" value="Art"></v-checkbox>
                            <v-checkbox v-model="selected" label="Sports" value="Sports"></v-checkbox>
                            <v-checkbox v-model="selected" label="Miscellaneous" value="Miscellaneous"></v-checkbox>
                            <v-checkbox v-model="selected" label="Educational" value="Educational"></v-checkbox>
                            <v-checkbox v-model="selected" label="Food" value="Food"></v-checkbox>
                            <v-checkbox v-model="selected" label="Discussion" value="Discussion"></v-checkbox>
                            </v-container>
                        </v-card-text>
                        <v-divider></v-divider>
                        <v-card-actions>
                            <v-btn color="blue darken-1" flat @click="dialog = false">Save</v-btn>
                        </v-card-actions>
                    </v-card>
                </v-dialog>
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
                <v-btn color="primary" flat @click="submit">Create Event</v-btn>
            </v-card-actions>
            </v-card>
        </v-flex>
        </v-layout>
    </v-app>
    </div>
</template>

<script>
/* eslint-disable */
    var todaysDate = new Date();
    import axios from 'axios'
import { error } from 'util';

    export default {
        name: 'create-event',
        data: () => ({
        errorMessages: '',
        name: null,
        address: null,
        city: null,
        state: null,
        zip: null,
        description: '',
        date: null,
        menu: false,
        timeMenu: false,
        dialog: false,
        startTime: null,
        formHasErrors: false,
        selected: [],
        ip: ''
        }),
        computed: {
            form () {
                return {
                    name: this.name,
                    address: this.address,
                    city: this.city,
                    state: this.state,
                    zip: this.zip,
                    description: this.description,
                    date: new Date(),
                    startTime: this.startTime
                }
            },
            minDate () {
                var dd = todaysDate.getDate();
                var mm = todaysDate.getMonth() + 1;
                var yyyy = todaysDate.getFullYear();

                if (dd < 10) {
                dd = "0" + dd;
                }
                if (mm < 10) {
                mm = "0" + mm;
                }

                var stringDate = yyyy + "-" + mm + "-" + dd;
                return stringDate;
            },
            minTime () {
                var hh = todaysDate.getHours();
                var mm = todaysDate.getMinutes();

                var timeString = hh + ":" + mm;
                return timeString;
            }
        },
        mounted () {
            axios({method: "GET", "url": "https://httpbin.org/ip" }).then(result => {
                this.ip = result.data.origin;
            }, error => {
                console.error(error);
            });
        },
        watch: {
            name () {
                this.errorMessages = ''
            },
            address () {
                this.errorMessages = ''
            },
            city () {
                this.errorMessages = ''
            },
            state () {
                this.errorMessages = ''
            },
            zip () {
                this.errorMessages = ''
            },
            date () {
                this.errorMessages = ''
            },
            time () {
                this.errorMessages = ''
            }
        },

        methods: {
            allowedDates: val => parseInt(val.split("-")) >= 0,
            startAllowedHours: v => v % 1 === 0,
            startAllowedStep: m => m % 10 === 0 || m % 10 === 5,
            titleCheck () {
                this.errorMessages = this.address && !this.name
                ? 'You must enter an event name' : ''
                return true
            },
            addressCheck () {
                this.errorMessages = this.name && !(this.address || this.city || this.state || this.zip)
                 ? 'You must enter an address' : ''
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
                if(this.formHasErrors === false) {
                    var eventStartTime = this.form.startTime.split(':');
                    var eventStartDateTime = new Date(this.form.date.getFullYear(), this.form.date.getMonth(), 
                    this.form.date.getDate(), eventStartTime[0], eventStartTime[1]);
                    var eventTagsSelected = this.selected;

                    axios.post("http://localhost:62008/api/event/createevent",
                        {
                            userId: 1,
                            startDate: eventStartDateTime,
                            eventName: this.form.name,
                            address: this.form.address,
                            city: this.form.city,
                            state: this.form.state,
                            zip: this.form.zip,
                            eventTags: eventTagsSelected,
                            eventDescription: this.form.description,
                            ip: this.ip,
                            url: "http://www.greetngroup.com/CreateEvent"
                        }).then((response) => {
                        if(response != null) {
                            alert("Your event has been created! Redirecting.");
                            this.$router.push('Welcome');
                        }
                        else {
                            alert("There was a problem creating your event. Redirecting.");
                            this.$router.push('Welcome');
                        }
                    }).catch(error => console.log(error));

                }
            }
        }
    }
    
</script>