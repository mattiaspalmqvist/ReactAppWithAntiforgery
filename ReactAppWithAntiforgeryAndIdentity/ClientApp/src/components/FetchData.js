import React, { Component } from 'react';
import authService from './api-authorization/AuthorizeService'
import antiforgeryService from './AntiforgeryService';

export class FetchData extends Component {
    static displayName = FetchData.name;

    constructor(props) {
        super(props);
        this.state = { forecasts: [], loading: true };
    }

    componentDidMount() {
        this.populateWeatherData();
    }

    static renderForecastsTable(forecasts) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Date</th>
                        <th>Temp. (C)</th>
                        <th>Temp. (F)</th>
                        <th>Summary</th>
                    </tr>
                </thead>
                <tbody>
                    {forecasts.map(forecast =>
                        <tr key={forecast.date}>
                            <td>{forecast.date}</td>
                            <td>{forecast.temperatureC}</td>
                            <td>{forecast.temperatureF}</td>
                            <td>{forecast.summary}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : FetchData.renderForecastsTable(this.state.forecasts);

        return (
            <div>
                <h1 id="tabelLabel" >Weather forecast</h1>
                <p>This component demonstrates fetching data from the server.</p>
                {contents}
            </div>
        );
    }

    async populateWeatherData() {
        let token = await authService.getAccessToken();
        const response = await fetch('WeatherForecast/PostWeather', {
            method: 'POST',
            headers: !token ?
                new Headers({
                    'Accept': 'application/json',
                    'RequestVerificationToken': antiforgeryService.getCookie("X-CSRF-FORM-TOKEN")
                }) :
                new Headers({
                    'Authorization': `Bearer ${token}`,
                    'Accept': 'application/json',
                    'RequestVerificationToken': antiforgeryService.getCookie("X-CSRF-FORM-TOKEN")
                })
        });
        const data = await response.json();
        this.setState({ forecasts: data, loading: false });
    }
}
